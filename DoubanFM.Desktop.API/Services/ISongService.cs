using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface ISongService
    {
        Task<PlayList> GetPlayList(int channelId);

        Task<PlayList> Like(string sid, int channelId);

        Task<PlayList> Unlike(string sid, int channelId);

        Task<PlayList> Skip(string sid, int channelId);

        Task<PlayList> NormalEnd(string sid, int channelId);

        Task<PlayList> Ban(string sid, int channelId);

    }

}
