using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.Slack.API
{
    [DataContract]
    public class SlackCommandResponseApi
    {
        /*
         {
    "text": "It's 80 degrees right now.",
    "attachments": [
        {
            "text":"Partly cloudy today and tomorrow"
        }
    ]
}
        */

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}
