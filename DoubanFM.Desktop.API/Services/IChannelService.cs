using DoubanFM.Desktop.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface IChannelService
    {
        Task<ChannelGroupList> GetAppChannels();
        Task<ChannelGroupList> GetHotChannels();

        Task<ChannelGroupList> GetUpTrendingChannels();

        Task<ChannelGroupList> GetRecommendChannels();

        Task<ChannelGroupList> GetRecentChannels();

        Task<ChannelGroupList> GetChannelInfo(int channelId);

    }
}
