using ApolloBot.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApolloBot.Other
{
    public class OtherApi : IApi
    {
        private DateTime _start;

        public OtherApi()
        {
            _start = DateTime.Now;
        }

        public void Init(IConfigurationRoot configuration)
        {

        }

        public IEnumerable<BotAction> GetActions()
        {
            var listActions = new List<BotAction>();

            var action = new BotAction()
            {
                CommandLine = Constants.CMD_SLITHER, Description = Constants.DESC_NO, Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<http://slither.io/>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_YAYA,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://media.giphy.com/media/3oKGz8CjdhZx1OCDV6/source.gif>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_UPTIME,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return $"Bot démarré depuis le {_start.ToString("dd/MM/yy HH:mm:ss")}";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_ROLL,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=dQw4w9WgXcQ>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_HORSE,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=OWFBqiUgspg>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_STARS,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=cl4ySbLvdEM>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_CHICKEN,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=rA9Ood3-peg>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_TAUPE,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=24pUKRQt7fk>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_FROG,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=k85mRPqvMbE>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_LOVE,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=Jr9R9NT9lk8>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_GITHUB,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "Vous pouvez contribuer ici : <https://github.com/ApolloSSC/ApolloBot>";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_SCRIBE,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "Vous savez, moi je ne crois pas qu’il y ait de bonne ou de mauvaise situation. Moi, si je devais résumer ma vie aujourd’hui avec vous, je dirais que c’est d’abord des rencontres. Des gens qui m’ont tendu la main, peut-être à un moment où je ne pouvais pas, où j’étais seul chez moi. Et c’est assez curieux de se dire que les hasards, les rencontres forgent une destinée... Parce que quand on a le goût de la chose, quand on a le goût de la chose bien faite, le beau geste, parfois on ne trouve pas l’interlocuteur en face je dirais, le miroir qui vous aide à avancer. Alors ça n’est pas mon cas, comme je disais là, puisque moi au contraire, j’ai pu : et je dis merci à la vie, je lui dis merci, je chante la vie, je danse la vie... je ne suis qu’amour ! Et finalement, quand beaucoup de gens aujourd’hui me disent « Mais comment fais-tu pour avoir cette humanité ? », et bien je leur réponds très simplement, je leur dis que c’est ce goût de l’amour ce goût donc qui m’a poussé aujourd’hui à entreprendre une construction mécanique, mais demain qui sait ? Peut-être simplement à me mettre au service de la communauté, à faire le don, le don de soi... ";
                }
            };
            listActions.Add(action);

            action = new BotAction()
            {
                CommandLine = Constants.CMD_SHARK,
                Description = Constants.DESC_NO,
                Category = Constants.CAT_OTHER,
                Execute = async (parameters, log, currentReader, user, time) =>
                {
                    return "<https://www.youtube.com/watch?v=XqZsoesa55w>";
                }
            };
            listActions.Add(action);

            return listActions;
        }

        public IEnumerable<BotReader> GetReaders()
        {
            return null;
        }

        
    }
}
