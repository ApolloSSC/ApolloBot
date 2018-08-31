using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Apollo.Weather.Models
{
    [DataContract]
    public class ResultWeekData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "list")]
        public List<ResultDayData> List { get; set; }

        [DataMember(Name = "city")]
        public City City { get; set; }

    }
}
