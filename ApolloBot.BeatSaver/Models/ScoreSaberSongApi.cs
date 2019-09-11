using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class ScoreSaberSongApi
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "songAuthorName")]
        public string SongAuthorName { get; set; }

        [DataMember(Name = "levelAuthorName")]
        public string LevelAuthorName { get; set; }
    }
}
