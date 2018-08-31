using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Apollo.Weather.Models
{
    [DataContract]
    public class City
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }
    }
}
