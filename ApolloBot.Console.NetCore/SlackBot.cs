using Apollo.Weather;
using Apollo.Youtube;
using ApolloBot.BeatSaver;
using ApolloBot.Cast;
using ApolloBot.Core;
using ApolloBot.Kaamelott;
using ApolloBot.Slack.API;
using log4net;
using Microsoft.Extensions.Configuration;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApolloBot.Console
{
    public class SlackBot
    {
        public static string BOT_RUNNING = "Démarrage du bot";
        public static string BOT_STOPPING = "Arrêt du bot";

        private string _webhook;
        private string _channelId;
        private string _channelName;
        private string _token;
        private string _botName;
        
        private Thread _threadBot;
        private bool _running;
        private SlackMessageApi _firstMessage;
        private SlackMessageApi _lastMessage;
        private DateTime _start;

        private ActionProcessor _processor;
        private SlackApi _api;

        private System.Action _onClose;

        private ILog _logger;

        private Process _vlcProcess;

        public SlackBot(IEnumerable<IApi> apis, IConfigurationRoot config, string webhook, string channelId, string channelName, string botName, string token, ILog logger, System.Action onClose)
        {
            _logger = logger;
            _channelId = channelId;
            _channelName = channelName;
            _botName = botName;
            _token = token;
            _webhook = webhook;
            _start = DateTime.Now;
            _onClose = onClose;

            _api = new SlackApi(_webhook);

            _processor = new ActionProcessor((text) =>
            {
                _logger.Debug(text);
//#if DEBUG
//                _api.SendMessage(_channelName, text, Emoji.Ghost, _botName, _logger);
//#endif
            });

            RegisterApi(apis, config);
        }

        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void RegisterApi(IEnumerable<IApi> apis, IConfigurationRoot config)
        {
            foreach(var api in apis)
            {
                api.Init(config);

                var actions = api.GetActions();
                if(actions != null)
                {
                    _processor.RegisterActions(actions);
                }

                var readers = api.GetReaders();
                if (readers != null)
                {
                    _processor.RegisterReaders(readers);
                }
            }
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
                                var result = _processor.ProcessString(message.Text, message.User, UnixTimeStampToDateTime(long.Parse(message.TimeStamp.Replace(".", "")) / 1000000));

                                if(result != null)
                                {
                                    var lines = result.Result.Split('\n');

                                    if(result.Result.StartsWith("{"))
                                    {
                                        _api.UploadFile(_channelName, _token, Encoding.UTF8.GetBytes(result.Result), message.TimeStamp, _logger);
                                    }
                                    else if(lines.Length > 25)
                                    {
                                        int count = 0;

                                        while (count < lines.Length)
                                        {
                                            int remain = lines.Length - count;
                                            int tempCount = remain > 25 ? 25 : remain;
                                            var res = _api.SendMessage2(_token, _channelName, string.Join("\n", lines.Skip(count).Take(tempCount)), Emoji.Ghost, _botName, _logger, null, message.TimeStamp, true).Result;
                                            count += tempCount;
                                        }
                                    }
                                    else
                                    {
                                        _api.SendMessage(_channelName, result.Result, Emoji.Ghost, _botName, _logger);
                                    }
                                }
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

        public void PostStartBot()
        {
            _logger.Info(BOT_RUNNING);
            _api.SendMessage(_channelName, BOT_RUNNING, Emoji.Ghost, _botName, _logger);
        }

        public void PostStopBot()
        {
            _logger.Info(BOT_STOPPING);
            _api.SendMessage(_channelName, BOT_STOPPING, Emoji.Ghost, _botName, _logger);

        }

    }
}
