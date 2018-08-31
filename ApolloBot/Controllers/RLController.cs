using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApolloBot.RocketLeague.API;
using ApolloBot.RocketLeague.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApolloBot.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RLController : Controller
    {
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;

        public RLController(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger("RLController");
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

        public async Task<Player> GetPlayerById(string id)
        {
            _logger.LogDebug("GetPlayerById");

            var api = new RLApi();
            var result = await api.GetPlayerById(id, 1);

            if (result != null)
            {
                return result;
            }

            return null;
        }
    }
}
