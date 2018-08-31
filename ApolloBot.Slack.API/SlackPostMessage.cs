using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.Slack.API
{
    [DataContract]
    public class SlackPostMessage
    {
        //{"type":"message","user":"UA3869VNF","text":"pffff nimp","client_msg_id":"8e0d0397-8750-4a70-adeb-e10ebae5c71b","ts":"1529515134.000594"}

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "mrkdwn")]
        public bool Mrkdwn { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "reply_broadcast")]
        public bool ReplyBroadcast { get; set; }

        [DataMember(Name = "thread_ts")]
        public string ThreadTs { get; set; }
    }
}
