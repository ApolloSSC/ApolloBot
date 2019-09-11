using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.BeatSaver.Models
{
    [DataContract]
    public class Playlist
    {
        [DataMember(Name = "playlistTitle")]
        public string Title { get; set; }

        [DataMember(Name = "playlistAuthor")]
        public string Author { get; set; }

        [DataMember(Name = "image")]
        public string Image { get; set; }

        [DataMember(Name = "songs")]
        public List<PlaylistSong> Songs { get; set; }
    }
}
