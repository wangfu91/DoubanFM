using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.API.Models
{
    [JsonObject("user")]
    public class User
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("played_num")]
        public int PlayedNum { get; set; }

        [JsonProperty("liked_num")]
        public int LikedNum { get; set; }

        [JsonProperty("banned_num")]
        public int BannedNum { get; set; }

        public bool IsPro { get; set; }

        public ProRate ProRate { get; set; }

        [JsonProperty("pro_status")]
        public string ProStatus { get; set; }

        [JsonProperty("pro_expire_date")]
        public string ProExpireDate { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

    }

}
