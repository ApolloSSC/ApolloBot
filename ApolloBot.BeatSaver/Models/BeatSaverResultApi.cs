using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class BeatSaverResultApi
    {
        [DataMember(Name = "docs")]
        public BeatSaverSongApi[] Songs { get; set; }

        [DataMember(Name = "song")]
        public BeatSaverSongApi Song { get; set; }
    }
}
