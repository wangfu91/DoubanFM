using Newtonsoft.Json;
using System.Collections.Generic;

namespace DoubanFM.Desktop.API.Models
{
    public class ChannelList
    {
        [JsonProperty("channels")]
        public List<Channel> Channels { get; set; }
    }

    [JsonObject]
    public class Channel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("seq_id")]
        public int SeqId { get; set; }

        [JsonProperty("abbr_en")]
        public string AbbrEN { get; set; }

        [JsonProperty("channel_id")]
        public int ChannelId { get; set; }

        [JsonProperty("name_en")]
        public string NameEN { get; set; }

    }

}
