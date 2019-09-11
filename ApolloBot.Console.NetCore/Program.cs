using Apollo.Weather;
using Apollo.Youtube;
using ApolloBot.BeatSaver;
using ApolloBot.Cast;
using ApolloBot.Core;
using ApolloBot.Kaamelott;
using ApolloBot.Other;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
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
        private static readonly ILog _log = log4net.LogManager.GetLogger(typeof(Program));

        public static bool _running = true;

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo(Directory.GetCurrentDirectory() + @"\log4net.config"));

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
               .AddJsonFile("appsettings.Debug.json", optional: true, reloadOnChange: true)
#else
               .AddJsonFile("appsettings.Release.json", optional: true, reloadOnChange: true)
#endif
               .AddUserSecrets<Program>()
               .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();


            try
            {
                string webhook = configuration.GetValue<string>("Slack:Webhook");
                string token = configuration.GetValue<string>("Slack:Token");
                string channelId = configuration.GetValue<string>("Slack:ChannelId");
                string channelName = configuration.GetValue<string>("Slack:ChannelName");
                string botName = configuration.GetValue<string>("Slack:ChannelId");

                _log.Info("Start");

                var slackBot = new SlackBot(
                    new List<IApi>()
                    {
                        new BeatSaverApi(),
                        new KaamelottApi(),
                        new WeatherApi(),
                        new ChromeCastApi(),
                        new OtherApi(),
                    },
                    configuration,
                    webhook, channelId, channelName, botName, token, _log, () => _running = false);

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
