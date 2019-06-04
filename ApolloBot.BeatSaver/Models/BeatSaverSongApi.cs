using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class BeatSaverSongApi
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

        [DataMember(Name = "authorName")]
        public string AuthorName { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "downloadCount")]
        public int DownloadCount { get; set; }

        [DataMember(Name = "playedCount")]
        public int PlayedCount { get; set; }

        [DataMember(Name = "upVotes")]
        public int UpVotes { get; set; }

        [DataMember(Name = "downVotes")]
        public int DownVotes { get; set; }

        [DataMember(Name = "downloadUrl")]
        public string DownloadUrl { get; set; }

        [DataMember(Name = "bpm")]
        public int Bpm { get; set; }

        [DataMember(Name = "coverUrl")]
        public string CoverUrl { get; set; }

        [DataMember(Name = "difficulties")]
        public Dictionary<string, BeatSaverDifficultyLevelApi> Difficulties { get; set; }
    }
}
