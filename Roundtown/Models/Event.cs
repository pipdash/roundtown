using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Roundtown.Models
{
    public class Event
    {
        public string ID { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool deleted { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }
        public string location { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public string photoID { get; set; }
    }
}
