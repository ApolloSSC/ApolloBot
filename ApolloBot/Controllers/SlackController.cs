using ApolloBot.RocketLeague.API;
using ApolloBot.Slack.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloBot.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SlackController : Controller
    {
        private const string CHANNEL = "CBSERNTKN";

        private ILoggerFactory _loggerFactory;
        private ILogger _logger;

        public SlackController(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger("SlackController");
        }

        // GET api/values
        //[HttpGet]
        //public async Task<IEnumerable<Player>> GetPlayerByName(string name)
        //{
        //    var api = new RLAPI();
        //    var result = await api.GetPlayerByName(name);

        //    if(result != null)
        //    {
        //        return result.Data;
        //    }

        //    return null;
        //}

        [HttpPost]
        public async Task Command(string command, string text, string response_url)
        {
            _logger.LogDebug($"[Command] {command}, {text}, {response_url}");
            if (command.ToLowerInvariant() == "stats")
            {
                var api = new RLApi();
                var result = await api.GetPlayerById(text, 1);

                if (result != null)
                {
                    var slack = new SlackApi(response_url);

                    slack.SendMessage(CHANNEL, result.SignatureUrl, Emoji.Ghost, "Apollo RocketLeague");
                }
            }
        }
    }
}
