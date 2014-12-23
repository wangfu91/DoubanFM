
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
        private ChannelSvcParams channelSvcParams;

        public ChannelService()
        {
            channelSvcParams = new ChannelSvcParams();
        }

        public ChannelService(LoginResult loginResult)
        {
            channelSvcParams = new ChannelSvcParams
            {
                user_id = loginResult.UserId,
                token = loginResult.Token,
                expire = loginResult.Expire
            };
        }

        public async Task<ChannelList> GetChannels()
        {
            return await Get<ChannelList>(ChannelRequestPath, channelSvcParams);
        }

        public async Task<SongResult> GetSongs(string channel)
        {
            channelSvcParams.channel = channel;
            channelSvcParams.type = "n";
            return await Get<SongResult>(SongRequestPath, channelSvcParams);
        }
    }
}
