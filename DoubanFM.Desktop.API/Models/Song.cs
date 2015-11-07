using Newtonsoft.Json;
using System.Collections.Generic;

namespace DoubanFM.Desktop.API.Models
{
    /// <summary>
    /// 歌曲信息
    /// </summary>
    [JsonObject("song")]
    public class Song : ModelBase
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// 专辑图片地址
        /// </summary>
        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("alert_msg")]
        public string AlertMessage { get; set; }

        /// <summary>
        /// 专辑名
        /// </summary>
        [JsonProperty("albumtitle")]
        public string AlbumTitle { get; set; }

        [JsonProperty("singers")]
        public List<Singer> Singers { get; set; }

        [JsonProperty("file_ext")]
        public string FileExt { get; set; }

        private bool like = false;

        /// <summary>
        /// 是否已喜欢
        /// </summary>
        [JsonProperty("like")]
        [JsonConverter(typeof(BoolConverter))]
        public bool Like
        {
            get
            {
                return like;
            }
            set
            {
                if (value != like)
                {
                    like = value;
                    NotifyPropertyChanged("Like");
                }
            }
        }

        /// <summary>
        /// 专辑地址
        /// </summary>
        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("ver")]
        public int Ver { get; set; }


        /// <summary>
        /// SSID
        /// </summary>
        [JsonProperty("ssid")]
        public string SSID { get; set; }

        /// <summary>
        /// 歌曲名
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 歌曲URL
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; set; }


        /// <summary>
        /// 艺术家
        /// </summary>
        [JsonProperty("artist")]
        public string Artist { get; set; }

        /// <summary>
        /// 子类型 (普通音乐应该是""，广告应该是"T")
        /// </summary>
        [JsonProperty("subtype")]
        public string SubType { get; set; }

        [JsonProperty("length")]
        public uint Length { get; set; }

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

    }

    public class Singer
    {
        [JsonProperty("related_site_id")]
        public int RelatedSiteId { get; set; }

        [JsonProperty("is_site_artist")]
        public bool IsSiteAtist { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

}
