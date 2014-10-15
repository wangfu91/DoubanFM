
using System.Collections.Generic;
namespace DoubanFM.Data
{
    /// <summary>
    /// 播放列表
    /// </summary>
    public class PlayList
    {
        public int R { get; set; }

        public int VersionMax { get; set; }

        public int IsShowQuickStart { get; set; }

        public List<Song> Song { get; set; }

    }
}
