using DoubanFM.Desktop.API.Models;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class SongService : ServiceBase, ISongService
    {
        private SongParams songParams;

        public SongService()
        {
            songParams = new SongParams();
        }

        public SongService(LoginResult loginResult)
        {
            songParams = new SongParams
            {
                user_id = loginResult.UserId,
                token = loginResult.Token,
                expire = loginResult.Expire
            };
        }

        public async Task<SongResult> GetSongs(int channel)
        {
            songParams.channel = channel.ToString();
            songParams.type = "n";
            return await Post<SongResult>(SongRequestPath, songParams);
        }

        public async Task<SongResult> Like(string sid, int channel)
        {
            songParams.sid = sid;
            songParams.channel = channel.ToString();
            songParams.type = "r";
            return await Get<SongResult>(SongRequestPath, songParams);
        }

        public async Task<SongResult> Unlike(string sid, int channel)
        {
            songParams.sid = sid;
            songParams.channel = channel.ToString();
            songParams.type = "u";
            return await Get<SongResult>(SongRequestPath, songParams);
        }

        public async Task<SongResult> Ban(string sid, int channel)
        {
            songParams.sid = sid;
            songParams.channel = channel.ToString();
            songParams.type = "b";
            return await Get<SongResult>(SongRequestPath, songParams);
        }

        public async Task<SongResult> Skip(string sid, int channel)
        {
            songParams.sid = sid;
            songParams.channel = channel.ToString();
            songParams.type = "s";
            return await Get<SongResult>(SongRequestPath, songParams);
        }

        public async Task<SongResult> NormalEnd(string sid, int channel)
        {
            songParams.sid = sid;
            songParams.channel = channel.ToString();
            songParams.type = "e";
            return await Get<SongResult>(SongRequestPath, songParams);
        }

    }

}
