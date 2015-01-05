using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class ChannelService : ServiceBase, IChannelService
    {
        private ChannelParams channelParams;

        public ChannelService()
        {
            channelParams = new ChannelParams();
        }

        public ChannelService(LoginResult loginResult)
        {
            channelParams = new ChannelParams
            {
                user_id = loginResult.UserId,
                token = loginResult.Token,
                expire = loginResult.Expire
            };
        }

        public async Task<ChannelList> GetChannels()
        {
            return await Get<ChannelList>(ChannelRequestPath, channelParams);
        }

    }

}
