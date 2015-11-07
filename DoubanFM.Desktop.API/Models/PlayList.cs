using Newtonsoft.Json;
using System.Collections.Generic;

namespace DoubanFM.Desktop.API.Models
{
    /// <summary>
    /// 播放列表
    /// </summary>
    public class PlayList
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
