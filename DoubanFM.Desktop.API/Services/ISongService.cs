using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public interface ISongService
    {
        Task<SongResult> GetSongs(string channelId);

        Task<SongResult> Like(string sid, string channelId);

        Task<SongResult> Unlike(string sid, string channelId);

        Task<SongResult> Skip(string sid, string channelId);

        Task<SongResult> NormalEnd(string sid, string channelId);

        Task<SongResult> Ban(string sid, string channelId);

    }

}
