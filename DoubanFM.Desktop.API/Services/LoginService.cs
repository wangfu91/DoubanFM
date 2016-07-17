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
        private new const string BaseUrl = "https://www.douban.com/";
        private const string AuthPath = "service/auth2/token";

        public async Task<LoginResult> LoginWithEmail(string email, string password)
        {
            using (var client = new HttpClient())
            {
                var paramSet = new Dictionary<string, string>
                {
                    { "client_id", "01620243a8d2134d042606cafa7639e7" },
                    { "client_secret", "e58172ce41cc2f58" },
                    { "redirecat_uri", "http://douban.fm/misc/win_proxy"},
                    {"grant_type", "password" },
                    {"password", password },
                    { "username", email }
                };

                var url = BuildRequestUri(BaseUrl, AuthPath, paramSet);
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", USER_AGENT);
                var stringContent = new StringContent("");
                stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await client.PostAsync(url, stringContent);

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LoginResult>(content);
            }
        }

    }

}
