using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Models
{
    public class SongResult
    {
        [JsonProperty("r")]
        public int R { get; set; }

        [JsonProperty("version_max")]
        public int VersionMax { get; set; }

        [JsonProperty("is_show_quick_start")]
        public int IsShowQuickStart { get; set; }

        [JsonProperty("song")]
        public List<Song> Songs { get; set; }

    }

}
