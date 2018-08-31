using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Apollo.Weather.Models
{
    [DataContract]
    public class ResultDayData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "weather")]
        public List<DetailedData> Weather { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "main")]
        public Main Main { get; set; }

        [DataMember(Name = "wind")]
        public Wind Wind { get; set; }

        [DataMember(Name = "dt")]
        public long DateRaw { get; set; }
    }

    [DataContract]
    public class DetailedData
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "main")]
        public string Main { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }

    [DataContract]
    public class Main
    {
        [DataMember(Name = "temp")]
        public double Temp { get; set; }

        [DataMember(Name = "temp_min")]
        public double TempMin { get; set; }

        [DataMember(Name = "temp_max")]
        public double TempMax { get; set; }

        [DataMember(Name = "humidity")]
        public double Humidity { get; set; }
    }


    [DataContract]
    public class Wind
    {
        [DataMember(Name = "speed")]
        public double Speed { get; set; }
    }
}
