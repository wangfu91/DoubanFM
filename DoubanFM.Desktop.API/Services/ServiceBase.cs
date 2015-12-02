using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public abstract class ServiceBase
    {
        protected const string BaseUrl = "https://api.douban.com/v2/fm/";
        protected const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0";

        private readonly Dictionary<string, string> ConstantParamSet = new Dictionary<string, string>
        {
            {"app_name", "radio_win8" },
            {"version", "1" },
            {"from", "s:win8|y:win8desktop|f:1" },
            {"context", "fmwin8app" },
            {"apikey", "01620243a8d2134d042606cafa7639e7" }
        };

        protected async Task<T> SendRequestAsync<T>(Uri requestUri, string accessToken, HttpMethod method)
        {

            using (var request = new HttpRequestMessage(method, requestUri))
            {
                request.Headers.Add("User-Agent", USER_AGENT);
                if (!string.IsNullOrEmpty(accessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue(accessToken);

                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(content).ToObject<T>();
                }
            }

        }


        protected Uri BuildRequestUri(string baseUrl, string reqPath, Dictionary<string, string> paramSet)
        {
            IEnumerable<KeyValuePair<string, string>> fullParamSet;
            if (paramSet != null)
                fullParamSet = paramSet.Union(ConstantParamSet);
            else
                fullParamSet = ConstantParamSet;

            var sb = new StringBuilder(baseUrl);
            sb.Append(reqPath);
            sb.Append("?");
            foreach (var param in fullParamSet)
            {
                var encodedKey = WebUtility.UrlEncode(param.Key);
                var encodedValue = WebUtility.UrlEncode(param.Value);
                sb.Append(encodedKey)
                    .Append("=")
                    .Append(encodedValue)
                    .Append("&");
            }
            sb.Remove(sb.Length - 1, 1);

            return new Uri(sb.ToString());
        }

    }

}
