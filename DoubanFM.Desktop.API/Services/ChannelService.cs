using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class ChannelService : ServiceBase, IChannelService
    {
        private const string channelNoDJ = "channels";
        private const string hotChannels = "hot_channels";
        private const string upTrendingChannels = "up_trending_channels";
        private const string recChannels = "rec_channels";
        private const string recentChannels = "recent_channels";

        private ChannelParams channelParams;

        public ChannelService()
        {
            channelParams = new ChannelParams();
        }

        public ChannelService(LoginResult loginResult)
        {
            channelParams = new ChannelParams
            {
                user_id = loginResult.DoubanUserId,
                token = loginResult.AccessToken,
                expire = loginResult.ExpireIn
            };
        }

        public async Task<ChannelList> GetChannels()
        {
            return await Get<ChannelList>(channelNoDJ, channelParams);
        }

    }

}
