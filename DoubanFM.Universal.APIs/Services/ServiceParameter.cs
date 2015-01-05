using DoubanFM.Universal.APIs.Models;

namespace DoubanFM.Universal.APIs.Services
{
    public class ParamsBase
    {
        public string app_name
        {
            get { return "radio_desktop_win"; }
        }

        public string version
        {
            get { return "100"; }
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

        public string expire { get; set; }

        public UserParams()
        {

        }

        public UserParams(LoginResult loginResult)
        {
            this.user_id = loginResult.UserId;
            this.token = loginResult.Token;
            this.expire = loginResult.Expire;
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
