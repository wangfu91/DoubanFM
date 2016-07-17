using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Models
{
    /// <summary>
    /// Lyric
    /// </summary>
    public class Lyrics :ModelBase 
    {
        [JsonProperty("lyric")]
        public string Lyric { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sid")]
        public string Sid { get; set; }

    }
}
