using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public abstract class ServiceBase
    {
        protected const string BaseUrl = "https://api.douban.com/v2/fm/";
        protected const string Legacy_BASE_URL = "http://www.douban.com/";

        protected const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:42.0) Gecko/20100101 Firefox/42.0";


        public const string LoginRequestPath = "j/app/login";
        public const string UserInfoRequestPath = "/j/app/radio/user_info";

        protected async Task<T> Get<T>(string path, ParamsBase param)
        {
            var restClient = new RestClient(BaseUrl);
            var request = new RestRequest(path, Method.GET);
            GetParameters(param).ForEach(p => request.AddParameter(p.Name, p.GetValue(param)));
            var response = await restClient.ExecuteTaskAsync(request);
            return Serialize<T>(response);
        }


        protected async Task<T> Post<T>(string path, ParamsBase param)
        {
            var restClient = new RestClient(BaseUrl);
            var request = new RestRequest(path, Method.POST);
            GetParameters(param).ForEach(p => request.AddParameter(p.Name, p.GetValue(param)));
            var response = await restClient.ExecuteTaskAsync(request);
            return Serialize<T>(response);
        }

        private T Serialize<T>(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                return JObject.Parse(content).ToObject<T>();
            }

            return default(T);
        }

        private List<PropertyInfo> GetParameters(ParamsBase param)
        {
            var type = param.GetType();
            var props = type.GetProperties();
            return props.Where(p => p.GetValue(param) != null).ToList();
        }

        protected Uri BuildUrl(string baseUrl, Dictionary<string,string> paramSet)
        {
            var sb = new StringBuilder(baseUrl);
            if(paramSet !=null && paramSet.Count > 0)
            {
                sb.Append("?");
                foreach (var param in paramSet)
                {
                    var encodedKey = WebUtility.UrlEncode(param.Key);
                    var encodedValue = WebUtility.UrlEncode(param.Value);
                    sb.Append(encodedKey)
                        .Append("=")
                        .Append(encodedValue)
                        .Append("&");
                }
                sb.Remove(sb.Length - 1, 1);
            }

            return new Uri(sb.ToString());
        }

    }

}
