using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.Kaamelott.Models
{
    [DataContract]
    public class Sound
    {
        [DataMember(Name = "character")]
        public string Character { get; set; }

        [DataMember(Name = "file")]
        public string File { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "episode")]
        public string Episode { get; set; }

        public int Id { get; set; }
    }
}
