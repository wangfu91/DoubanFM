using DoubanFM.Universal.APIs.Models;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public interface IChannelService
    {
        Task<ChannelList> GetChannels();
    }
}
