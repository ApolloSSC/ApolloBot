using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ApolloBot.RocketLeague.API.Models
{
    [DataContract]
    public class Result<T>
    {
        [DataMember(Name = "page")]
        public int Page { get; set; }

        [DataMember(Name = "results")]
        public int Results { get; set; }

        [DataMember(Name = "totalResults")]
        public int TotalResults { get; set; }

        [DataMember(Name = "maxResultsPerPage")]
        public int MaxResultsPerPage { get; set; }

        [DataMember(Name = "data")]
        public T Data { get; set; }
    }
}
