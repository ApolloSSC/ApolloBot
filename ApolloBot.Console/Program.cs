using Apollo.Weather;
using Apollo.Youtube;
using ApolloBot.BeatSaver;
using ApolloBot.Cast;
using ApolloBot.Kaamelott;
using ApolloBot.RocketLeague.API;
using log4net.Config;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApolloBot.Console
{
    class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(Program));

        public static bool _running = true;

        public static string _channelId = "CC0RDM3ME";
        //public static string _channelId = "CBSERNTKN";

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo(Directory.GetCurrentDirectory() + @"\log4net.config"));

            try
            {
                string webhook = null;
                string token = null;
                string channelId = _channelId;
                if (args != null)
                {
                    if (args.Length > 0)
                    {
                        webhook = args[0];
                    }
                    if (args.Length > 1)
                    {
                        token = args[1];
                    }
                    if (args.Length > 2)
                    {
                        channelId = args[2];
                    }
                }

                _log.Info("Start");

                var slackApi = new Slack.API.SlackApi(null);
                var ytApi = new YoutubeApi(_log);
                var rlApi = new RLApi();
                var weatherApi = new WeatherApi();
                var kaamelottApi = new KaamelottApi();
                var chromeCastApi = new ChromeCastApi();
                var beatSaverApi = new BeatSaverApi();


                var slackBot = new SlackBot(slackApi, ytApi, rlApi, weatherApi, kaamelottApi, beatSaverApi, chromeCastApi, _channelId,
                    "xoxp-301409525076-303733381060-426903598116-8bf538b93cad3c84ec7ede55edaa11f0", _log, () => _running = false);

                slackBot.Start();

//#if DEBUG
//                string command = Constants.CMD_WEATHER_DAY;
//                slackBot.ExecuteCommand("", command);
//#endif

                while (_running)
                {
                    Thread.Sleep(1000);
                }

                slackBot.Stop();

                _log.Info("End");

            }
            catch (Exception e)
            {
                _log.Error(e);
            }
            
        }
    }
}
