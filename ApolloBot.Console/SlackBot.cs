using Apollo.Weather;
using Apollo.Youtube;
using ApolloBot.Cast;
using ApolloBot.Kaamelott;
using ApolloBot.RocketLeague.API;
using ApolloBot.Slack.API;
using log4net;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApolloBot.Console
{
    public class SlackBot
    {
        public static string BOT_RUNNING = "Démarrage du bot";
        public static string BOT_STOPPING = "Arrêt du bot";


        private Dictionary<string, Action<string, string[], string>> _commands;
        private SlackApi _api;
        private YoutubeApi _ytApi;
        private RLApi _rlApi;
        private WeatherApi _weatherApi;
        private KaamelottApi _kaamelottApi;
        private ChromeCastApi _castApi;
        private string _channelId;
        private string _token;
        private Thread _threadBot;
        private bool _running;
        private SlackMessageApi _firstMessage;
        private SlackMessageApi _lastMessage;
        private DateTime _start;
        private Dictionary<string, string> _rlIds = new Dictionary<string, string>()
        {
            { "U9FHDFW2J", "76561198090050306" }, //gperouffe
            { "U8XMKB71S", "76561198045055811" }, //Platou
            { "UA3869VNF", "76561198074900071" }, //AlexisBaron
            { "U8X0V00GK", "approcheputt" } //Remy
        };

        private string _botName = "ApolloBot";
        private string _channel = "#bot";
        //private string _channel = "#rocketleague";
        private Action _onClose;

        private ILog _logger;

        public SlackBot(SlackApi api, YoutubeApi ytApi, RLApi rlApi, WeatherApi weatherApi, KaamelottApi kaamelottApi, ChromeCastApi castApi, string channelId, string token, ILog logger, Action onClose)
        {
            _logger = logger;
            _api = api;
            _ytApi = ytApi;
            _rlApi = rlApi;
            _weatherApi = weatherApi;
            _kaamelottApi = kaamelottApi;
            _castApi = castApi;
            _channelId = channelId;
            _token = token;
            _start = DateTime.Now;
            _onClose = onClose;

            _commands = new Dictionary<string, Action<string, string[], string>>()
            {
                { Constants.CMD_SLITHER, (user, parameters, ts) => { _api.SendMessage(channelId, "<http://slither.io/>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_ALEXIS, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://image.ibb.co/gZCMnJ/money.jpg>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_YAYA, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://media.giphy.com/media/3oKGz8CjdhZx1OCDV6/source.gif>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_UPTIME, (user, parameters, ts) => { _api.SendMessage(channelId, $"Bot démarré depuis le {_start.ToString("dd/MM/yy HH:mm:ss")}", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_ROLL, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=dQw4w9WgXcQ>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_PUDDY, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=KyucG76N9PY>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_HORSE, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=OWFBqiUgspg>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_POIREAU, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=jdL-K9EgSwE>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_STARS, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=cl4ySbLvdEM>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_CHICKEN, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=rA9Ood3-peg>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_TAUPE, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=24pUKRQt7fk>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_FROG, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=k85mRPqvMbE>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_LOVE, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://www.youtube.com/watch?v=Jr9R9NT9lk8>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_GITHUB, (user, parameters, ts) => { _api.SendMessage(channelId, "Vous pouvez contribuer ici : <https://github.com/ApolloSSC/ApolloBot>", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_SCRIBE, (user, parameters, ts) => { _api.SendMessage(channelId, "Vous savez, moi je ne crois pas qu’il y ait de bonne ou de mauvaise situation. Moi, si je devais résumer ma vie aujourd’hui avec vous, je dirais que c’est d’abord des rencontres. Des gens qui m’ont tendu la main, peut-être à un moment où je ne pouvais pas, où j’étais seul chez moi. Et c’est assez curieux de se dire que les hasards, les rencontres forgent une destinée... Parce que quand on a le goût de la chose, quand on a le goût de la chose bien faite, le beau geste, parfois on ne trouve pas l’interlocuteur en face je dirais, le miroir qui vous aide à avancer. Alors ça n’est pas mon cas, comme je disais là, puisque moi au contraire, j’ai pu : et je dis merci à la vie, je lui dis merci, je chante la vie, je danse la vie... je ne suis qu’amour ! Et finalement, quand beaucoup de gens aujourd’hui me disent « Mais comment fais-tu pour avoir cette humanité ? », et bien je leur réponds très simplement, je leur dis que c’est ce goût de l’amour ce goût donc qui m’a poussé aujourd’hui à entreprendre une construction mécanique, mais demain qui sait ? Peut-être simplement à me mettre au service de la communauté, à faire le don, le don de soi... ", Emoji.Ghost, _botName, _logger); } },
                { Constants.CMD_STOP, (user, parameters, ts) => {
                        _api.SendMessage(channelId, $"Demande d'arrêt du bot...", Emoji.Ghost, _botName, _logger);
                        _onClose?.Invoke();
                    }
                },
                { Constants.CMD_LIST, (user, parameters, ts) => {
                        _api.SendMessage(channelId, string.Join(", ", _commands.Keys), Emoji.Ghost, _botName, _logger);
                    }
                },
                { Constants.CMD_YOUTUBE, async (user, parameters, ts) => {
                        if(parameters.Length == 1)
                        {
                            var video = await _ytApi.GetVideo(parameters[0]);
                            if(video == null)
                            {
                                _api.SendMessage(channelId, $"Video {parameters[0]} non trouvée", Emoji.Ghost, _botName, _logger);
                            }
                            else
                            {
                                var image = video.Snippet.Thumbnails.Maxres?.Url;
                                
                                if(image == null)
                                {
                                    image = video.Snippet.Thumbnails.Standard?.Url;
                                }

                                if(image == null)
                                {
                                    image = video.Snippet.Thumbnails.Medium?.Url;
                                }

                                if(image == null)
                                {
                                    image = video.Snippet.Thumbnails.Default__?.Url;
                                }

                                // if(image == null)
                                //{
                                    _api.SendMessage(channelId, $"*{video.Snippet.Title}* par *{video.Snippet.ChannelTitle}*\n<https://www.youtube.com/watch?v={parameters[0]}>",
                                        Emoji.Ghost, _botName, _logger, null);
                                //}
                                // else
                                //{
                                //    _api.SendMessage(channelId, $"*{video.Snippet.Title}* par *{video.Snippet.ChannelTitle}*\n<https://www.youtube.com/watch?v={parameters[0]}>",
                                //        Emoji.Ghost, _botName, _logger, new List<SlackAttachment>() { new SlackAttachment() { Title = video.Snippet.Title, ImageUrl = image } });
                                //}
                                
                            }
                        }
                        else
                        {
                            _api.SendMessage(channelId, $"La commande ne prend qu'un seul paramètre, l'id de la vidéo", Emoji.Ghost, _botName, _logger);
                        }
                    }
                },
                { Constants.CMD_ROCKETLEAGUE, async (user, parameters, ts) => {
                        if(parameters.Length == 0)
                        {
                            if(!_rlIds.ContainsKey(user))
                            {
                                _api.SendMessage(channelId, $"Vous n'avez pas d'id lié", Emoji.Ghost, _botName, _logger);
                                return;
                            }

                            var player = await _rlApi.GetPlayerById(_rlIds[user], 1);
                            if(player == null)
                            {
                                _api.SendMessage(channelId, $"Player {_rlIds[user]} non trouvé", Emoji.Ghost, _botName, _logger);
                            }
                            else
                            {
                                _api.SendMessage(channelId, $"<{player.SignatureUrl}>", Emoji.Ghost, _botName,
                                    _logger);
                            }
                        }
                        else if(parameters.Length == 1)
                        {
                            

                            var player = await _rlApi.GetPlayerById(parameters[0], 1);
                            if(player == null)
                            {
                                _api.SendMessage(channelId, $"Player {parameters[0]} non trouvé", Emoji.Ghost, _botName, _logger);
                            }
                            else
                            {
                                _api.SendMessage(channelId, $"<{player.SignatureUrl}>", Emoji.Ghost, _botName, 
                                    _logger);
                            }
                        }
                        else
                        {
                            _api.SendMessage(channelId, $"La commande ne prend qu'un seul paramètre, l'id de la vidéo", Emoji.Ghost, _botName, _logger);
                        }
                    }
                },
                { Constants.CMD_ROCKETLEAGUE_MEHDI, async (user, parameters, ts) => {
                        var player = await _rlApi.GetPlayerById("Sopalin Brulant", 3);
                        if(player == null)
                        {
                            _api.SendMessage(channelId, $"Player {parameters[0]} non trouvé", Emoji.Ghost, _botName, _logger);
                        }
                        else
                        {
                            _api.SendMessage(channelId, $"<{player.SignatureUrl}>", Emoji.Ghost, _botName,
                                _logger);
                        }
                    }
                },
                { Constants.CMD_ROCKETLEAGUE_APOLLO, async (user, parameters, ts) => {
                        var player = await _rlApi.GetPlayerById("76561198333340183", 1);
                        if(player == null)
                        {
                            _api.SendMessage(channelId, $"Player {parameters[0]} non trouvé", Emoji.Ghost, _botName, _logger);
                        }
                        else
                        {
                            _api.SendMessage(channelId, $"<{player.SignatureUrl}>", Emoji.Ghost, _botName,
                                _logger);
                        }
                    }
                },
                { Constants.CMD_WEATHER, async (user, parameters, ts) => {
                        var day = await _weatherApi.GetCurrentDay(parameters.Length >= 1 ? parameters[0] : null);
                        if(day == null)
                        {
                            _api.SendMessage(channelId, $"Une erreur est survenue", Emoji.Ghost, _botName, _logger);
                        }
                        else
                        {
                            _api.SendMessage(channelId, day, Emoji.Ghost, _botName,
                                _logger);
                        }
                    }
                },
                 { Constants.CMD_WEATHER_DAY, async (user, parameters, ts) => {
                        var week = await _weatherApi.GetCurrentWeek(parameters.Length >= 1 ? parameters[0] : null);
                        if(week == null)
                        {
                            _api.SendMessage(channelId, $"Une erreur est survenue", Emoji.Ghost, _botName, _logger);
                        }
                        else
                        {
                            _api.SendMessage(channelId, week, Emoji.Ghost, _botName, _logger);
                        }
                    }
                },
                 { Constants.CMD_KAAMELOTT_SITE, (user, parameters, ts) => { _api.SendMessage(channelId, "<https://kaamelott-soundboard.2ec0b4.fr/>", Emoji.Ghost, _botName, _logger); } },
                  { Constants.CMD_KAAMELOTT, async (user, parameters, ts) => {
                      if(parameters.Length == 0)
                      {
                           await _api.SendMessage2(_token, channelId, $"La commande  obligatoirement un paramètre, le texte à chercher", Emoji.Ghost, _botName, _logger, null, ts, true);
                      }
                      else
                      {
                            var sounds = await _kaamelottApi.Search(parameters.Length >= 1 ? parameters[0] : null);
                            if(!sounds.Any())
                            {
                                await _api.SendMessage2(_token, channelId, $"Pas de sons trouvés", Emoji.Ghost, _botName, _logger, null, ts, true);
                            }
                            else
                            {
                                await _api.SendMessage2(_token, channelId, string.Join("\n", sounds), Emoji.Ghost, _botName, _logger, null, ts, true);
                            }
                      } 
                    }
                },
                   { Constants.CMD_KAAMELOTT_PLAY, async (user, parameters, ts) => {
                      if(parameters.Length == 0)
                      {
                           _api.SendMessage(channelId, $"La commande  obligatoirement un paramètre, le texte à chercher", Emoji.Ghost, _botName, _logger);
                      }
                      else
                      {
                           var param = parameters[0];
                           int paramId = 0;

                           var result = int.TryParse(param , out paramId);

                           if(result)
                           {
                                
                                //var resultPlay = await _kaamelottApi.PlayById(paramId);
                                var sound = await _kaamelottApi.GetMp3(paramId);

                                if(sound == null)
                               {
                                   _api.SendMessage(channelId, $"Le son n'a pas été trouvé pour l'id {paramId}", Emoji.Ghost, _botName, _logger);
                               }
                               else
                               {
                                   await _castApi.Send(sound);
                               } 
                           }
                           else
                           {
                                var resultPlay = await _kaamelottApi.Play(param);

                                if(!resultPlay)
                                {
                                    _api.SendMessage(channelId, $"Le son n'a pas été trouvé pour le nom {parameters[0]}", Emoji.Ghost, _botName, _logger);
                                }
                           }
                       }
                    }
                },
                   { Constants.CMD_KAAMELOTT_LIST, async (user, parameters, ts) => {
                      var result = await _kaamelottApi.GetAll();

                        if(result.Any())
                        {
                            var str = "";

                            int cpt = 0;

                            foreach(var sound in result)
                           {
                               str += $"*Id:* {sound.Id} | *Titre:* {sound.Title} | *Perso:* {sound.Character} | *Episode:* {sound.Episode}\n";
                               cpt++;

                               if(cpt % 25 == 0)
                               {
                                   await _api.SendMessage2(_token, channelId, str, Emoji.Ghost, _botName, _logger, null, ts, true);
                                   str = "";
                               }
                           }

                            await _api.SendMessage2(_token, channelId, str, Emoji.Ghost, _botName, _logger, null, ts, true);
                        }
                   }
                },
                    { Constants.CMD_KAAMELOTT_VOLUME, async (user, parameters, ts) => {
                          if(parameters.Length == 0)
                          {
                               _api.SendMessage(channelId, $"La commande obligatoirement un paramètre, la valeur du volume entre 0 et 100", Emoji.Ghost, _botName, _logger);
                          }
                          else
                          {
                               var param = parameters[0];
                               int paramId = 0;

                               var result = int.TryParse(param , out paramId);

                               if(result)
                               {
                                    if(paramId < 0 || paramId > 100)
                                    {
                                        _api.SendMessage(channelId, $"La valeur n'est pas entre 0 et 100", Emoji.Ghost, _botName, _logger);
                                        return;
                                    }

                                    _kaamelottApi.SetVolume(paramId);
                               }
                               else
                               {
                                    _api.SendMessage(channelId, $"La valeur n'est pas un int", Emoji.Ghost, _botName, _logger);
                               }
                           }
                     }
                },
                     { Constants.CMD_CHROMECAST_FIND, async (user, parameters, ts) => {
                         var result = await _castApi.Find();
                               
                        if(result != null && result.Any())
                        {
                            _api.SendMessage(channelId, string.Join("\n", result), Emoji.Ghost, _botName, _logger);
                        }
                        else
                        {
                            _api.SendMessage(channelId, $"Aucun récepteur trouvé", Emoji.Ghost, _botName, _logger);
                        }
                     }
                },
                      { Constants.CMD_CHROMECAST_SEND, async (user, parameters, ts) => {
                          if(parameters.Length < 1)
                          {
                               _api.SendMessage(channelId, $"La commande obligatoirement 1 paramètre, l'url du média", Emoji.Ghost, _botName, _logger);
                          }
                          else
                          {
                              if(parameters.Length > 2)
                              {
                                  //var device = string.Join(" ", parameters.Take(parameters.Length - 1));
                                  var url = parameters.Last();

                                  var result = await _castApi.Send(url.Replace("<", "").Replace(">", ""));
                              }
                              else
                              {
                                var result = await _castApi.Send(parameters[0].Replace("<", "").Replace(">", ""));
                              }
                          }
                     }
                },
            };
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void Start()
        {
            _running = true;

            PostStartBot();

            _threadBot = new Thread(Run);
            _threadBot.Start();
        }

        public void Stop()
        {
            _running = false;

            PostStopBot();
        }

        public void Run()
        {
            while (_running)
            {
                try
                {
                    List<SlackMessageApi> messages = null;

                    if (_lastMessage == null)
                    {
                        messages = _api.GetMessagesFromChannel(_token, _channelId, 100).Result.ToList();
                    }
                    else
                    {
                        messages = _api.GetMessagesFromChannel(_token, _channelId, 100, _lastMessage.TimeStamp).Result.ToList();
                    }

                    if (_lastMessage == null)
                    {
                        _lastMessage = messages.Any() ? messages.FirstOrDefault() : null;
                    }
                    else
                    {
                        _lastMessage = messages.Any() ? messages.FirstOrDefault() : _lastMessage;

                        if (messages.Any())
                        {
                            foreach (var message in messages)
                            {
                                ExecuteCommand(message.User, message.Text, message.TimeStamp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }

                Thread.Sleep(1000);
            }
        }

        public void ExecuteCommand(string user, string commandAsked, string ts)
        {
            if (string.IsNullOrEmpty(commandAsked))
            {
                return;
            }

            var parseLine = commandAsked.Split(' ');
            var commandStr = parseLine[0].ToLowerInvariant();

            var command = _commands.Keys.Where(c => c == commandStr).FirstOrDefault();

            if (_commands.ContainsKey(commandStr))
            {
                _commands[commandStr].Invoke(user, parseLine.Skip(1).ToArray(), ts);
            }
        }

        public void PostStartBot()
        {
            _logger.Info(BOT_RUNNING);
            _api.SendMessage(_channel, BOT_RUNNING, Emoji.Ghost, _botName, _logger);
        }

        public void PostStopBot()
        {
            _logger.Info(BOT_STOPPING);
            _api.SendMessage(_channel, BOT_STOPPING, Emoji.Ghost, _botName, _logger);

        }

    }
}
