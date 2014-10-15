
using Newtonsoft.Json;
namespace DoubanFM.Data
{
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
        public string ChannelId { get; set; }

        [JsonProperty("name_en")]
        public string NameEN { get; set; }


    }
}
