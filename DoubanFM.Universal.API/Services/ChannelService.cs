using DoubanFM.Universal.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Universal.API.Services
{
    public class ChannelService : ServiceBase
    {
        private const string ChannelRequestPath = "j/app/radio/channels";
        private const string SongRequestPath = "j/app/radio/people";

        private ChannelParams channelParams;

        public ChannelService()
        {
            channelParams = new ChannelParams();
        }

        public ChannelService(LoginResult loginResult)
        {
            channelParams = new ChannelParams
            {
                user_id = loginResult.UserId,
                token = loginResult.Token,
                expire = loginResult.Expire
            };
        }

        public async Task<ChannelList> GetChannels()
        {
            return await Get<ChannelList>(ChannelRequestPath, channelParams);
        }

        public async Task<SongResult> GetSongs(string channel)
        {
            channelParams.channel = channel;
            channelParams.type = "n";
            return await Get<SongResult>(SongRequestPath, channelParams);
        }
    }

}
