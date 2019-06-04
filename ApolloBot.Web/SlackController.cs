using ApolloBot.Slack.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace ApolloBot.Web
{
    public class SlackController : ApiController
    {
        [HttpGet]
        public string Index()
        {
            return "Hello world";
        }

        [HttpPost]
        public JsonResult<SlackCommandResponseApi> PostCommand([FromBody] SlackCommandApi command)
        {
            return new JsonResult<SlackCommandResponseApi>(
                new SlackCommandResponseApi()
                {
                    Text = "It's 80 degrees right now."
                }, new Newtonsoft.Json.JsonSerializerSettings(), Encoding.UTF8, this);
        }
    }
}
