using Newtonsoft.Json;

namespace DoubanFM.Desktop.API.Models
{
    [JsonObject]
    public class LoginResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("douban_user_name")]
        public string DoubanUserName { get; set; }

        [JsonProperty("douban_user_id")]
        public string DoubanUserId { get; set; }

        [JsonProperty("expire_in")]
        public long ExpireIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

    }

}
