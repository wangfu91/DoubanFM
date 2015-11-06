using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace DoubanFM.Desktop.API.Services
{
    public class LoginService : ServiceBase, ILoginService
    {
        private const string baseUrl = "https://www.douban.com/service/auth2/token";

        public async Task<LoginResult> LoginWithEmail(string email, string password)
        {
            using (var client = new HttpClient())
            {
                var paramSet = new Dictionary<string, string>();
                paramSet.Add("client_id", "01620243a8d2134d042606cafa7639e7");
                paramSet.Add("client_secret", "e58172ce41cc2f58");
                paramSet.Add("redirecat_uri", "http://douban.fm/misc/win_proxy");
                paramSet.Add("grant_type", "password");
                paramSet.Add("password", password);
                paramSet.Add("username", email);

                var url = BuildUrl(baseUrl, paramSet);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.UserAgent.ParseAdd(USER_AGENT);
                request.Content = new StringContent("");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoginResult>(content);
                return result;
            }
        }


        public async Task<LoginResult> LoginWithUserName(string userName, string password)
        {
            var param = new LoginParams
            {
                username = userName,
                password = password
            };

            return await Post<LoginResult>(LoginRequestPath, param);
        }

    }

}
