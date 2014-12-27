using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public abstract class ServiceBase
    {
        protected const string baseUrl = "http://www.douban.com";
        protected const string ChannelReqPath = "j/app/radio/channels";
        protected const string SongReqPath = "j/app/radio/people";
        protected const string LoginReqPath = "j/app/login";
        protected const string UserReqPath = "/j/app/radio/user_info";


        protected async Task<T> Get<T>(string path,ParamsBase param)
        {
            var restClient = new RestClient(baseUrl);
            var request = new RestRequest(path, HttpMethod.Get);

            //ForEach method has been removed form WinRT, 
            //<see cref="http://stackoverflow.com/questions/10299458/is-the-listt-foreach-method-gone" >
            //GetParameters(param).ForEach(p => request.AddParameter(p.Name, p.GetValue(param)));

            foreach (var p in GetParameters(param))
            {
                request.AddParameter(p.Name, p.GetValue(param));                
            }
            var response = await restClient.Execute<T>(request);
            return response.Data;

        }

        protected async Task<T> Post<T>(string path, ParamsBase param)
        {
            var restClient = new RestClient(baseUrl);
            var request = new RestRequest(path, HttpMethod.Post);
            foreach (var p in GetParameters(param))
            {
                request.AddParameter(p.Name, p.GetValue(param));
            } 
            var response = await restClient.Execute<T>(request);
            return response.Data;
        }

  
        private List<PropertyInfo> GetParameters(ParamsBase param)
        {
            var type = param.GetType();

            //WinRT does not has this method as .net.
            //var props = type.GetProperties(); 

            //Note: this returns only the properties declared in the class itself.
            //not return the properties of it's parent classes.
            //Too bad, this is not what I want!
            //var props = type.GetType().GetTypeInfo().DeclaredProperties;

            //This is what I want, it returns all the properties.
            //<see cref=" http://dotnetbyexample.blogspot.com/2012/06/reflection-in-winrt-declaredproperties.html ">
            var props = type.GetRuntimeProperties();

            return props.Where(p => p.GetValue(param) != null).ToList();
        }
    }
}
