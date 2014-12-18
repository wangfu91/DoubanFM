
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

        public async Task<ChannelList> GetChannels()
        {
            return await Get<ChannelList>(ChannelRequestPath, new ChannelSvcParams());
        }

        public async Task<SongResult> GetSongs(string channel)
        {
            var param = new ChannelSvcParams
            {
                channel = channel,
                type = "n"
            };
            return await Get<SongResult>(SongRequestPath, param);
        }
    }
}
