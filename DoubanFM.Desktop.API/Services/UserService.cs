using DoubanFM.Desktop.API.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class UserService : ApiServiceBase, IUserService
    {
        private const string UserInfo = "user_info";

        public UserService() { }

        public async Task<User> GetUserInfo(string userId, string accessToken)
        {
            using (var client = new HttpClient())
            {
                var paramSet = new Dictionary<string, string>
                {
                    {"avatar_size","large" }                    
                };

                var requestUri = BuildRequestUri(BaseUrl, UserInfo, paramSet);
                return await SendRequestAsync<User>(requestUri,accessToken,HttpMethod.Get);               
            }

        }

    }

}
