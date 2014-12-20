
using DoubanFM.Data;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
namespace DoubanFM.Service
{
    public class SongService : BaseService
    {
        private SongSvcParams songSvcParams;

        public SongService(UserSvcParams userSvcParams)
        {
            songSvcParams = new SongSvcParams
            {
                user_id = userSvcParams.user_id,
                token = userSvcParams.token,
                expire = userSvcParams.expire
            };
        }


        public async Task<SongResult> Like(string sid,string channel)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "r";
            return await Get<SongResult>(SongRequestPath, songSvcParams);
        }

        public async Task<SongResult> Unlike(string sid,string channel)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "u";
            return await Get<SongResult>(SongRequestPath, songSvcParams);
        }

        public async Task<SongResult> Ban(string sid, string channel)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "b";
            return await Get<SongResult>(SongRequestPath, songSvcParams);
        }

        public async Task<SongResult> Skip(string sid,string channel)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "s";
            return await Get<SongResult>(SongRequestPath, songSvcParams);
        }

        public async Task<SongResult> NormalEnd(string sid, string channel)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "e";
            return await Get<SongResult>(SongRequestPath, songSvcParams);
        }
    }

}
