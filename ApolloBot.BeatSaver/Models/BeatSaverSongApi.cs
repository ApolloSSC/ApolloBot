using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class BeatSaverSongApi
    {
        [DataMember(Name = "metadata")]
        public BeatSaverSongMetadataApi Metadata { get; set; }

        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "stats")]
        public BeatSaverSongStatsApi Stats { get; set; }
    }
}
