using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface IChannelService
    {
        Task<ChannelList> GetChannels();
    }
}
