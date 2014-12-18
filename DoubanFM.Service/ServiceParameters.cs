
namespace DoubanFM.Service
{

    public abstract class BaseSvcParams
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

    public class LoginSvcParams : BaseSvcParams
    {
        public string email { get; set; }

        public string username { get; set; }

        public string password { get; set; }

    }

    public class UserSvcParams : BaseSvcParams
    {
        public string user_id { get; set; }

        public string token { get; set; }

        public string expire { get; set; }

    }

    public class ChannelSvcParams : UserSvcParams
    {
        public string channel { get; set; }
        public string type { get; set; }
    }

    public class SongSvcParams : ChannelSvcParams
    {
        public string sid { get; set; }
    }

}
