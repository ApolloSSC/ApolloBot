using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.RocketLeague.API.Models
{
    [DataContract]
    public class Player
    {
        [DataMember(Name = "uniqueId")]
        public string UniqueId { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "avatar")]
        public string Avatar { get; set; }

        [DataMember(Name = "profileUrl")]
        public string ProfileUrl { get; set; }

        [DataMember(Name = "signatureUrl")]
        public string SignatureUrl { get; set; }

        [DataMember(Name = "stats")]
        public PlayerStats Stats { get; set; }
    }

    [DataContract]
    public class PlayerStats
    {
        [DataMember(Name = "wins")]
        public int Wins { get; set; }

        [DataMember(Name = "goals")]
        public int Goals { get; set; }

        [DataMember(Name = "mvps")]
        public int Mvps { get; set; }

        [DataMember(Name = "saves")]
        public int Saves { get; set; }

        [DataMember(Name = "shots")]
        public int Shots { get; set; }

        [DataMember(Name = "assists")]
        public int Assists { get; set; }
    }

}
