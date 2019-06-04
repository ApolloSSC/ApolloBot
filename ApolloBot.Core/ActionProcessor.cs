using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloBot.Core
{
    public class ActionProcessor
    {
        private System.Action<string> _log;
        private List<BotAction> _actions;
        private List<BotReader> _readers;
        private BotReader _currentReader;

        public ActionProcessor(System.Action<string> log)
        {
            _log = log;

            _actions = new List<BotAction>();
            _readers = new List<BotReader>();

            RegisterActions(CreateReaderActions());
            RegisterActions(CreateCategoryActions());
        }

        public void RegisterActions(IEnumerable<BotAction> actions)
        {
            foreach(var action in actions)
            {
                if(_actions.Where(a => a.CommandLine == action.CommandLine).Any())
                {
                    _log($"Impossible d'ajouter la commande *{action.CommandLine}* car une action est déjà enregistrée pour cette commande");
                    continue;
                }

                _actions.Add(action);
                _log($"Action *{action.CommandLine}* ({action.Description}) ajoutée");
            }
        }

        public void RegisterReaders(IEnumerable<BotReader> readers)
        {
            foreach (var reader in readers)
            {
                if (_readers.Where(a => a.Name == reader.Name).Any())
                {
                    _log($"Impossible d'ajouter le lecteur *{reader.Name}* car un lecteur est déjà enregistré avec ce nom");
                    continue;
                }

                _readers.Add(reader);
                _log($"Lecteur *{reader.Name}* ajouté");
            }

            if(_currentReader == null && _readers.Any())
            {
                _currentReader = _readers.First();
            }
        }

        public Task<string> ProcessString(string text, string user = null, DateTime? date = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var parseLine = text.Split(' ');
            var commandStr = parseLine[0].ToLowerInvariant();

            var action = _actions.Where(a => a.CommandLine.ToLowerInvariant() == commandStr.ToLowerInvariant()).FirstOrDefault();

            if(action == null)
            {
                return null;
            }

            try
            {
                var parameters = parseLine.Skip(1).ToArray();

                if(parameters.Length > 0 && parameters.First().ToLowerInvariant() == "help")
                {
                    return Task.FromResult(action.Description);
                }

                return action.Execute(parameters, _log, _currentReader, user, date);
            }
            catch(Exception ex)
            {
                return Task.FromResult(ex.Message);
            }
        }

        #region Reader Actions
        private IEnumerable<BotAction> CreateReaderActions()
        {
            var listActions = new List<BotAction>();

            var setReaderAction = new BotAction()
            {
                CommandLine = Constants.CMD_READER_SET,
                Description = Constants.DESC_READER_SET,
                Category = Constants.CAT_OTHER,
                Execute = (parameters, log, currentReader, user, time) =>
                {
                    if (parameters == null || !parameters.Any())
                    {
                        return Task.FromResult($"Vous devez fournir le nom du lecteur");
                    }

                    var name = parameters[0];

                    var reader = _readers.Where(r => r.Name.ToLowerInvariant() == name.ToLowerInvariant()).FirstOrDefault();

                    if(reader == null)
                    {
                        return Task.FromResult($"Le lecteur avec le nom *{name}* n'a pas pu être trouvé");
                    }

                    if(_currentReader != null)
                    {
                        _currentReader.Stop(log);
                    }

                    _currentReader = reader;

                    return Task.FromResult($"Le lecteur courant est maintenant *{_currentReader.Name}*");
                }
            };
            listActions.Add(setReaderAction);

            var getReadersAction = new BotAction()
            {
                CommandLine = Constants.CMD_READER_GET_ALL,
                Description = Constants.DESC_READER_GET_ALL,
                Category = Constants.CAT_OTHER,
                Execute = (parameters, log, currentReader, user, time) =>
                {
                    if (!_readers.Any())
                    {
                        return Task.FromResult($"Il n'y a pas de lecteurs enregistrés");
                    }

                    return Task.FromResult(string.Join("\n", _readers.Select(r => r.Name).ToList()));
                }
            };
            listActions.Add(getReadersAction);

            var getReaderAction = new BotAction()
            {
                CommandLine = Constants.CMD_READER_GET,
                Description = Constants.DESC_READER_GET,
                Category = Constants.CAT_OTHER,
                Execute = (parameters, log, currentReader, user, time) =>
                {
                    if (_currentReader == null)
                    {
                        return Task.FromResult($"Il n'y a pas de lecteur sélectionné");
                    }

                    return Task.FromResult($"Le lecteur courant est *{_currentReader.Name}*");
                }
            };
            listActions.Add(getReaderAction);

            return listActions;
        }
        #endregion

        #region Category Actions
        private IEnumerable<BotAction> CreateCategoryActions()
        {
            var listActions = new List<BotAction>();

            var categoryAction = new BotAction()
            {
                CommandLine = Constants.CMD_CATEGORY_ACTIONS,
                Description = Constants.DESC_CATEGORY_ACTIONS,
                Category = Constants.CAT_OTHER,
                Execute = (parameters, log, currentReader, user, time) =>
                {
                    if (parameters == null || !parameters.Any())
                    {
                        return Task.FromResult($"Vous devez fournir la categorie");
                    }

                    var name = parameters[0];

                    var actions = _actions.Where(r => r.Category.ToLowerInvariant() == name.ToLowerInvariant()).ToList();

                    if (!actions.Any())
                    {
                        return Task.FromResult($"Il n'y a pas d'actions pour la catégorie *{name}*");
                    }

                    return Task.FromResult(string.Join("\n", actions.Select(a => $"*{a.CommandLine}* ({a.Description})")));
                }
            };
            listActions.Add(categoryAction);

            var categoriesAction = new BotAction()
            {
                CommandLine = Constants.CMD_CATEGORY_ALL,
                Description = Constants.DESC_CATEGORY_ALL,
                Category = Constants.CAT_OTHER,
                Execute = (parameters, log, currentReader, user, time) =>
                {
                    if (!_actions.Any())
                    {
                        return Task.FromResult($"Il n'y a pas d'actions d'enregistrées");
                    }

                    var categories = _actions.Select(a => $"*{a.Category}*").Distinct();

                    return Task.FromResult(string.Join("\n", categories));
                }
            };
            listActions.Add(categoriesAction);

            return listActions;
        }
        #endregion
    }
}
