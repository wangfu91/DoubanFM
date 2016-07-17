using DoubanFM.Desktop.API.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class ChannelService : ServiceBase, IChannelService
    {
        private const string LegacyChannels = "channels";
        private const string AppChannels = "app_channels";
        private const string HotChannels = "hot_channels";
        private const string UpTrendingChannels = "up_trending_channels";
        private const string RecommendChannels = "rec_channels";
        private const string RecentChannels = "recent_channels";
        private const string ChannelInfo = "channel_info";

        private string _accessToken = "";

        public string AccessToken
        {
            set { _accessToken = value; }
        }

        public async Task<ChannelGroupList> GetAppChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, AppChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, _accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetHotChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, HotChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, _accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetUpTrendingChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, UpTrendingChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, _accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetRecommendChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, RecommendChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, _accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetRecentChannels()
        {
            var requestUri = BuildRequestUri(BaseUrl, RecentChannels, null);
            return await SendRequestAsync<ChannelGroupList>(requestUri, _accessToken, HttpMethod.Get);
        }

        public async Task<ChannelGroupList> GetChannelInfo(int channelId)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"channel",channelId.ToString() }
            };
            var requestUri = BuildRequestUri(BaseUrl, RecentChannels, paramSet);
            return await SendRequestAsync<ChannelGroupList>(requestUri, _accessToken, HttpMethod.Get);
        }

    }

}
