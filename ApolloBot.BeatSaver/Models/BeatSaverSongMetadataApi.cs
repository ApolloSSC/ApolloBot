using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class BeatSaverSongMetadataApi
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "songName")]
        public string SongName { get; set; }

        [DataMember(Name = "songSubName")]
        public string SongSubName { get; set; }

        [DataMember(Name = "songAuthorName")]
        public string SongAuthorName { get; set; }

        [DataMember(Name = "levelAuthorName")]
        public string AuthorName { get; set; }

        [DataMember(Name = "bpm")]
        public int Bpm { get; set; }
    }
}
