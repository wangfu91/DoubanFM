using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DoubanFM.Service
{
    public abstract class BaseService
    {
        private const string baseUrl = "http://www.douban.com/";
        public BaseService()
        {

        }

        protected async Task<IRestResponse> Get(string path, ServiceParameter param)
        {
            var restClient = new RestClient(baseUrl);
            var request = new RestRequest(path, Method.GET);
            GetParameters(param).ForEach(p => request.AddParameter(p.Name, p.GetValue(param)));
            var response = await restClient.ExecuteTaskAsync(request);
            return response;
        }


        protected async Task<IRestResponse> Post(string path, ServiceParameter param)
        {
            var restClient = new RestClient(baseUrl);
            var request = new RestRequest(path, Method.POST);
            GetParameters(param).ForEach(p => request.AddParameter(p.Name, p.GetValue(param)));
            var response= await restClient.ExecuteTaskAsync(request);
            return response;
        }


        private List<PropertyInfo> GetParameters(ServiceParameter param)
        {
            var type = Type.GetType(param.ToString());
            var props = type.GetProperties();
            return props.Where(p => p.GetValue(param) != null).ToList();
        }

    }
}
