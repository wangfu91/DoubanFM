using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Models
{
    [JsonObject]
    public class Album
    {
        [JsonProperty("artists")]
        public string Artists { get; set; }

        [JsonProperty("rating")]
        public float Rating { get; set; }

        [JsonProperty("img")]
        public string Image { get; set; }

        [JsonProperty("tittle")]
        public string Title { get; set; }

        [JsonProperty("'date")]
        public string Date { get; set; }

        [JsonProperty("subject_id")]
        public string SubjectId { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("medias")]
        public string Medias { get; set; }
    }
}
