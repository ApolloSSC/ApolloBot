using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.Slack.API
{
    [DataContract]
    public class SlackCommandApi
    {
        /*
         token=gIkuvaNzQIHg97ATvDxqgjtO
        &team_id=T0001
        &team_domain=example
        &enterprise_id=E0001
        &enterprise_name=Globular%20Construct%20Inc
        &channel_id=C2147483705
        &channel_name=test
        &user_id=U2147483697
        &user_name=Steve
        &command=/weather
        &text=94070
        &response_url=https://hooks.slack.com/commands/1234/5678
        &trigger_id=13345224609.738474920.8088930838d88f008e0
        */

        [DataMember(Name = "token")]
        public string Token { get; set; }

        [DataMember(Name = "team_id")]
        public string TeamId { get; set; }

        [DataMember(Name = "team_domain")]
        public string TeamDomain { get; set; }

        [DataMember(Name = "channel_id")]
        public string ChannelId { get; set; }

        [DataMember(Name = "channel_name")]
        public string ChannelName { get; set; }

        [DataMember(Name = "user_id")]
        public string UserId { get; set; }

        [DataMember(Name = "user_name")]
        public string UserName { get; set; }

        [DataMember(Name = "command")]
        public string Command { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "response_url")]
        public string ResponseUrl { get; set; }

        [DataMember(Name = "trigger_id")]
        public string TriggerId { get; set; }
    }
}
