using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface ISongService
    {
        Task<SongResult> GetSongs(int channelId);

        Task<SongResult> Like(string sid, int channelId);

        Task<SongResult> Unlike(string sid, int channelId);

        Task<SongResult> Skip(string sid, int channelId);

        Task<SongResult> NormalEnd(string sid, int channelId);

        Task<SongResult> Ban(string sid, int channelId);

    }

}
