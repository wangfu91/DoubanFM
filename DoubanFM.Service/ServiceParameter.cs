
namespace DoubanFM.Service
{
    public class ServiceParameter
    {

        public ServiceParameter()
        {
            this.app_name = "radio_desktop_win";
            this.version = "100";
        }

        public string app_name { get; set; }

        public string version { get; set; }

        public string channel { get; set; }

        public string type { get; set; }

        public string sid { get; set; }

        public string email { get; set; }

        public string password { get; set; }

    }
}
