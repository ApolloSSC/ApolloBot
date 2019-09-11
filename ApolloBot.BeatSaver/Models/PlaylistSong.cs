using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class PlaylistSong
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "hash")]
        public string Hash { get; set; }

        [DataMember(Name = "songName")]
        public string SongName { get; set; }

        [DataMember(Name = "uploader")]
        public string Uploader { get; set; }
    }
}
