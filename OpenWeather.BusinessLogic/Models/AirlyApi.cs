using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeather.BusinessLogic.Models
{
    public class ResponsePollution
    {
        public PollutionData Current { get; set; }
    }

    public class PollutionData
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public List<Value> Values { get; set; }
        public List<Index> Indexes { get; set; }
    }

    public class Index
    {
        public string Description { get; set; }
        public string Color { get; set; }
    }


    public class Value
    {
        public string Name { get; set; }
        [JsonProperty("value")]
        public double PM25Value { get; set; }
    }
}
