using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class ScoreSaberResultApi
    {
        [DataMember(Name = "songs")]
        public ScoreSaberSongApi[] Songs { get; set; }
    }
}
