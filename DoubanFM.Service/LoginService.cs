using DoubanFM.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
namespace DoubanFM.Service
{
    public class LoginService : BaseService
    {
        public async Task<LogonInfo> Login(string email, string password)
        {
            var path = "j/app/login";

            var param = new ServiceParameter
            {
                email = email,
                password = password
            };

            var response = await Post(path, param);
            var userInfo = new LogonInfo();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                var jo = JObject.Parse(content);
                userInfo = jo.ToObject<LogonInfo>();
            }

            return userInfo;
        }

    }
}
