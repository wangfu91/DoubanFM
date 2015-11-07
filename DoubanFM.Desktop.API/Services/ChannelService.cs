using DoubanFM.Desktop.API.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class ChannelService : ServiceBase, IChannelService
    {
        private const string legacyChannels = "channels";
        private const string appChannels = "app_channels";
        private const string hotChannels = "hot_channels";
        private const string upTrendingChannels = "up_trending_channels";
        private const string recommendChannels = "rec_channels";
        private const string recentChannels = "recent_channels";
        private const string channelInfo = "channel_info";

        private string accessToken;

        public ChannelService(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public ChannelService() { }

        public async Task<ChannelGroupList> GetAppChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, appChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetHotChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, hotChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetUpTrendingChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, upTrendingChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetRecommendChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, recommendChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetRecentChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, recentChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetChannelInfo(int channelId)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"channel",channelId.ToString() }
            };
            var requestUri = BuildRequestUri(BaseUrl, recentChannels, paramSet);
            return await SendRequestAsync<ChannelGroupList>(requestUri, accessToken, HttpMethod.Get);
        }

    }

}
