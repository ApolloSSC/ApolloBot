using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class BeatSaverDifficultyLevelApi
    {
        [DataMember(Name = "difficulty")]
        public string Difficulty { get; set; }

        [DataMember(Name = "rank")]
        public int Rank { get; set; }

        [DataMember(Name = "audioPath")]
        public string AudioPath { get; set; }

        [DataMember(Name = "jsonPath")]
        public string JsonPath { get; set; }

        [DataMember(Name = "offset")]
        public string Offset { get; set; }

        [DataMember(Name = "old_offset")]
        public string OldOffset { get; set; }


    }
}
