
using DoubanFM.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
namespace DoubanFM.Service
{
    public class PlayListService : BaseService
    {

        public async Task<PlayList> SendRequest(string channel, string type, string sid)
        {
            var path = "j/app/radio/people";
            var param = new ServiceParameter
            {
                channel = channel,
                type = type,
                sid = sid
            };

            var response = await Get(path, param);
            var playList = new PlayList();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                playList = JObject.Parse(content).ToObject<PlayList>();
            }

            return playList;
        }

    }
}
