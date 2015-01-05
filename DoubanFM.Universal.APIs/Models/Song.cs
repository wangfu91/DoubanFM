using Newtonsoft.Json;

namespace DoubanFM.Universal.APIs.Models
{
    /// <summary>
    /// 歌曲信息
    /// </summary>
    [JsonObject("song")]
    public class Song : ModelBase
    {

        private string album;

        /// <summary>
        /// 专辑地址
        /// </summary>
        [JsonProperty("album")]
        public string Album
        {
            get
            {
                return album;
            }
            set
            {
                if (value != album)
                {
                    album = value;
                    NotifyPropertyChanged("Album");
                }
            }
        }



        private string picture;

        /// <summary>
        /// 专辑图片地址
        /// </summary>
        [JsonProperty("picture")]
        public string Picture
        {
            get
            {
                return picture;
            }
            set
            {
                if (value != picture)
                {
                    picture = value;
                    NotifyPropertyChanged("Picture");
                }
            }
        }


        private string ssid;

        /// <summary>
        /// SSID
        /// </summary>
        [JsonProperty("ssid")]
        public string SSID
        {
            get
            {
                return ssid;
            }
            set
            {
                if (value != ssid)
                {
                    ssid = value;
                    NotifyPropertyChanged("SSID");
                }
            }
        }



        private string artist;

        /// <summary>
        /// 艺术家
        /// </summary>
        [JsonProperty("artist")]
        public string Artist
        {
            get
            {
                return artist;
            }
            set
            {
                if (value != artist)
                {
                    artist = value;
                    NotifyPropertyChanged("Artist");
                }
            }
        }



        private string url;

        /// <summary>
        /// 歌曲URL
        /// </summary>
        [JsonProperty("url")]
        public string URL
        {
            get
            {
                return url;
            }
            set
            {
                if (value != url)
                {
                    url = value;
                    NotifyPropertyChanged("URL");
                }
            }
        }


        private string company;

        /// <summary>
        /// 唱片公司
        /// </summary>
        [JsonProperty("company")]
        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                if (value != company)
                {
                    company = value;
                    NotifyPropertyChanged("Company");
                }
            }
        }



        private string title;

        /// <summary>
        /// 歌曲名
        /// </summary>
        [JsonProperty("title")]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (value != title)
                {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }



        private double ratingAvg;

        /// <summary>
        /// 平均评级
        /// </summary>
        [JsonProperty("rating_avg")]
        public double RatingAvg
        {
            get
            {
                return ratingAvg;
            }
            set
            {
                if (value != ratingAvg)
                {
                    ratingAvg = value;
                    NotifyPropertyChanged("RatingAvg");
                }
            }
        }



        private uint length;

        /// <summary>
        /// 歌曲长度
        /// </summary>
        [JsonProperty("length")]
        public uint Length
        {
            get
            {
                return length;
            }
            set
            {
                if (value != length)
                {
                    length = value;
                    NotifyPropertyChanged("Length");
                }
            }
        }



        private string subType;

        /// <summary>
        /// 子类型 (普通音乐应该是""，广告应该是"T")
        /// </summary>
        [JsonProperty("subtype")]
        public string SubType
        {
            get
            {
                return subType;
            }
            set
            {
                if (value != subType)
                {
                    subType = value;
                    NotifyPropertyChanged("SubType");
                }
            }
        }



        private string publicTime;

        /// <summary>
        /// 出版年份
        /// </summary>
        [JsonProperty("public_time")]
        public string PublicTime
        {
            get
            {
                return publicTime;
            }
            set
            {
                if (value != publicTime)
                {
                    publicTime = value;
                    NotifyPropertyChanged("PublishTime");
                }
            }
        }



        private uint songListCount;

        /// <summary>
        /// 未知
        /// </summary>
        [JsonProperty("songlists_count")]
        public uint SongListCount
        {
            get
            {
                return songListCount;
            }
            set
            {
                if (value != songListCount)
                {
                    songListCount = value;
                    NotifyPropertyChanged("SongListCount");
                }
            }
        }


        private string sid;

        /// <summary>
        /// 歌曲Id
        /// </summary>
        [JsonProperty("sid")]
        public string SID
        {
            get
            {
                return sid;
            }
            set
            {
                if (value != sid)
                {
                    sid = value;
                    NotifyPropertyChanged("SID");
                }
            }
        }



        private string aid;

        /// <summary>
        /// 专辑Id
        /// </summary>
        [JsonProperty("aid")]
        public string AID
        {
            get
            {
                return aid;
            }
            set
            {
                if (value != aid)
                {
                    aid = value;
                    NotifyPropertyChanged("AID");
                }
            }
        }



        private string sha256;

        /// <summary>
        /// SHA
        /// </summary>
        [JsonProperty("sha256")]
        public string SHA256
        {
            get
            {
                return sha256;
            }
            set
            {
                if (value != sha256)
                {
                    sha256 = value;
                    NotifyPropertyChanged("SHA256");
                }
            }
        }



        private string kbps;

        /// <summary>
        /// 歌曲码率
        /// </summary>
        [JsonProperty("kbps")]
        public string Kbps
        {
            get
            {
                return kbps;
            }
            set
            {
                if (value != kbps)
                {
                    kbps = value;
                    NotifyPropertyChanged("Kbps");
                }
            }
        }



        private string albumTitle;

        /// <summary>
        /// 专辑名
        /// </summary>
        [JsonProperty("albumtitle")]
        public string AlbumTitle
        {
            get
            {
                return albumTitle;
            }
            set
            {
                if (value != albumTitle)
                {
                    albumTitle = value;
                    NotifyPropertyChanged("AlbumTitle");
                }
            }
        }



        private bool like = false;

        /// <summary>
        /// 是否已喜欢
        /// </summary>
        [JsonProperty("like")]
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


    }

}
