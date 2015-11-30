using Newtonsoft.Json;
using System.Collections.Generic;

namespace DoubanFM.Desktop.API.Models
{
    public class ChannelGroupList
    {
        [JsonProperty("groups")]
        public List<ChannelGroup> Groups { get; set; }
    }

    public class ChannelGroup
    {
        [JsonProperty("chls")]
        public List<Channel> Channels { get; set; }

        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        [JsonProperty("group_name")]
        public string GroupName { get; set; }
    }

    public class Channel
    {
        [JsonProperty("style")]
        public Style Style { get; set; }

        [JsonProperty("intro")]
        public string Intro { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("song_num")]
        public int SongNum { get; set; }

        [JsonProperty("collected")]
        public string Collected { get; set; }

        [JsonProperty("cover")]
        public string Cover { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("pro_desc")]
        public string ProDesc { get; set; }

        [JsonProperty("brand_icon")]
        public string BrandIcon { get; set; }

        [JsonProperty("brand_logo")]
        public string BrandLogo { get; set; }

        [JsonProperty("brand_bg")]
        public string BrandBg { get; set; }
    }

    public class Style
    {

        [JsonProperty("display_text")]
        public string DisplayText { get; set; }

        [JsonProperty("bg_color")]
        public string BgColor { get; set; }

        [JsonProperty("layout_type")]
        public int LayoutType { get; set; }

        [JsonProperty("bg_image")]
        public string BgImage { get; set; }
    }


    public class SearchChannels
    {
        [JsonProperty("channels")]
        public List<Channel> Channels { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }   

}
