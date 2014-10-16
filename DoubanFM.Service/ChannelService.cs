
using DoubanFM.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
namespace DoubanFM.Service
{
    public class ChannelService : BaseService
    {

        public async Task<List<Channel>> GetChannels(int channel, string type)
        {
            var path = "j/app/radio/channels";
            var param = new ServiceParameter
            {
                channel = channel.ToString(),
                type = type 
            };
            var response = await Get(path, param);
            var channels = new List<Channel>();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                var jo = JObject.Parse(content)["channels"];
                channels = jo.ToObject<List<Channel>>();
            }
            return channels;
        }
    }
}
