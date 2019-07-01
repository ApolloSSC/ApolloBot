using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class BeatSaverSongStatsApi
    {
        [DataMember(Name = "downloads")]
        public int DownloadCount { get; set; }

        [DataMember(Name = "plays")]
        public int PlayedCount { get; set; }

        [DataMember(Name = "upVotes")]
        public int UpVotes { get; set; }

        [DataMember(Name = "downVotes")]
        public int DownVotes { get; set; }

        [DataMember(Name = "rating")]
        public float Rating { get; set; }

        [DataMember(Name = "heat")]
        public float Heat { get; set; }
    }
}
