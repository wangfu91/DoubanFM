using DoubanFM.Desktop.API.Models;

namespace DoubanFM.Desktop.API.Services
{
    public class ParamsBase
    {
        public string app_name
        {
            get { return "radio_win8"; }
        }

        public string version
        {
            get { return "1"; }
        }

        public string from
        {
            get { return "s:win8|y:win8desktop|f:1"; }
        }

        public string context
        {
            get { return "fmwin8app"; }
        }

        public string apikey
        {
            get { return "01620243a8d2134d042606cafa7639e7"; }
        }
    }

    public class LoginParams : ParamsBase
    {
        public string email { get; set; }

        public string username { get; set; }

        public string password { get; set; }

    }

    public class UserParams : ParamsBase
    {
        public string user_id { get; set; }

        public string token { get; set; }

        public long expire { get; set; }

        public UserParams()
        {

        }

        public UserParams(LoginResult loginResult)
        {
            this.user_id = loginResult.DoubanUserId;
            this.token = loginResult.AccessToken;
            this.expire = loginResult.ExpireIn;
        }

    }

    public class ChannelParams : UserParams
    {
        public string channel { get; set; }
        public string type { get; set; }
    }

    public class SongParams : ChannelParams
    {
        public string sid { get; set; }
    }
}
