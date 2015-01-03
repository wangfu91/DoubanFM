using DoubanFM.Universal.APIs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.APIs.Services
{
    public class SongService : ServiceBase,ISongService
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

        public async Task<SongResult> GetSongs(string channel)
        {
            songParams.channel = channel;
            songParams.type = "n";
            return await Post<SongResult>(SongReqPath, songParams);
        }

        public async Task<SongResult> Like(string sid, string channel)
        {
            songParams.sid = sid;
            songParams.channel = channel;
            songParams.type = "r";
            return await Get<SongResult>(SongReqPath, songParams);
        }

        public async Task<SongResult> Unlike(string sid, string channel)
        {
            songParams.sid = sid;
            songParams.channel = channel;
            songParams.type = "u";
            return await Get<SongResult>(SongReqPath, songParams);
        }

        public async Task<SongResult> Ban(string sid, string channel)
        {
            songParams.sid = sid;
            songParams.channel = channel;
            songParams.type = "b";
            return await Get<SongResult>(SongReqPath, songParams);
        }

        public async Task<SongResult> Skip(string sid, string channel)
        {
            songParams.sid = sid;
            songParams.channel = channel;
            songParams.type = "s";
            return await Get<SongResult>(SongReqPath, songParams);
        }

        public async Task<SongResult> NormalEnd(string sid, string channel)
        {
            songParams.sid = sid;
            songParams.channel = channel;
            songParams.type = "e";
            return await Get<SongResult>(SongReqPath, songParams);
        }

    }
}
