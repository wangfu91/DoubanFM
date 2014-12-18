
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


        public async Task<bool> Like(string sid,string channel)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "r";
            var result = await Get<SongResult>(SongRequestPath, songSvcParams);
            return result != null && result.R == 0;
        }

        public async Task<bool> Unlike(string sid,string channel, string userId, string token, string expire)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "u";
            var result = await Get<SongResult>(SongRequestPath, songSvcParams);
            return result != null && result.R == 0;
        }

        public async Task<bool> Ban(string sid, string channel)
        {
            songSvcParams.sid = sid;
            songSvcParams.channel = channel;
            songSvcParams.type = "b";
            var result = await Get<SongResult>(SongRequestPath, songSvcParams);
            return result != null && result.R == 0;
        }

    }

}
