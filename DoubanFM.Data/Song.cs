
using Newtonsoft.Json;
namespace DoubanFM.Data
{
    /// <summary>
    /// 歌曲信息
    /// </summary>
    [JsonObject("song")]
    public class Song
    {
        /// <summary>
        /// 专辑地址
        /// </summary>
        [JsonProperty("album")]
        public string Album { get; set; }

        /// <summary>
        /// 专辑图片地址
        /// </summary>
        [JsonProperty("picture")]
        public string Picture { get; set; }

        /// <summary>
        /// SSID
        /// </summary>
        [JsonProperty("ssid")]
        public string SSID { get; set; }

        /// <summary>
        /// 艺术家
        /// </summary>
        [JsonProperty("artist")]
        public string Artist { get; set; }

        /// <summary>
        /// 歌曲URL
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; set; }

        /// <summary>
        /// 唱片公司
        /// </summary>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>
        /// 歌曲名
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 平均评级
        /// </summary>
        [JsonProperty("rating_avg")]
        public double RatingAvg { get; set; }

        /// <summary>
        /// 歌曲长度
        /// </summary>
        [JsonProperty("length")]
        public uint Length { get; set; }

        /// <summary>
        /// 子类型 (普通音乐应该是""，广告应该是"T")
        /// </summary>
        [JsonProperty("subtype")]
        public string SubType { get; set; }

        /// <summary>
        /// 出版年份
        /// </summary>
        [JsonProperty("public_time")]
        public string PublicTime { get; set; }

        /// <summary>
        /// 未知
        /// </summary>
        [JsonProperty("songlists_count")]
        public uint SongListsCount { get; set; }

        /// <summary>
        /// 歌曲Id
        /// </summary>
        [JsonProperty("sid")]
        public string SID { get; set; }

        /// <summary>
        /// 专辑Id
        /// </summary>
        [JsonProperty("aid")]
        public string AID { get; set; }

        /// <summary>
        /// SHA
        /// </summary>
        [JsonProperty("sha256")]
        public string SHA256 { get; set; }

        /// <summary>
        /// 歌曲码率
        /// </summary>
        [JsonProperty("kbps")]
        public string Kbps { get; set; }

        /// <summary>
        /// 专辑名
        /// </summary>
        [JsonProperty("albumtitle")]
        public string AlbumTitle { get; set; }

        /// <summary>
        /// 是否已喜欢
        /// </summary>
        [JsonProperty("like")]
        public uint Like { get; set; }


    }
}
