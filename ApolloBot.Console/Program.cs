using Apollo.Weather;
using Apollo.Youtube;
using ApolloBot.Cast;
using ApolloBot.Kaamelott;
using ApolloBot.RocketLeague.API;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


                var slackBot = new SlackBot(slackApi, ytApi, rlApi, weatherApi, kaamelottApi, chromeCastApi, _channelId, 
                    "xoxp-301409525076-303733381060-409456340035-ac577a7a1589305fc2776395a82e4418", _log, () => _running = false);

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
