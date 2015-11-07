using Newtonsoft.Json;

namespace DoubanFM.Desktop.API.Models
{
    public class User
    {
        [JsonProperty("programme_collected_num")]
        public int ProgrammeCollectedNum { get; set; }


        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("played_num")]
        public int PlayedNum { get; set; }

        [JsonProperty("channel_collected_num")]
        public int ChannelCollectedNum { get; set; }


        [JsonProperty("pro_status")]
        public string ProStatus { get; set; }

        [JsonProperty("pro_expire_date")]
        public string ProExpireDate { get; set; }


        [JsonProperty("liked_num")]
        public int LikedNum { get; set; }

        [JsonProperty("banned_num")]
        public int BannedNum { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        public bool IsPro
        {
            get { return !ProStatus.Equals("N"); }
        }

    }

}
