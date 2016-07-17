using DoubanFM.Desktop.API.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Services
{
    public class SongService : ServiceBase, ISongService
    {

        /*
 type : 
    'n' 开启电台第一次请求列表，或者切换频道后第一次请求列表。返回播放列表，客户端应立即播放该列表。
    'e' 报告一首歌播放完成, 仅报告用，不返回播放列表。客户端不对返回值做处理。
    'p' 报告播放完成, 并返回新的列表。用于当前列表播放完毕（或即将播放完毕）时。返回播放列表，客户端应立即播放新列表。
    's' 跳过。返回播放列表，客户端应立即停止当前歌曲，并播放新列表。
    'r' 喜欢。DJ频道播放旁白时不可用。返回播放列表，客户端应立即用新列表替换当前播放列表, 但继续播放当前歌曲。
    'u' 取消喜欢。DJ频道播放旁白时不可用。返回播放列表，客户端应立即用新列表替换当前播放列表, 但继续播放当前歌曲。
    'b' 不再播放。仅限私人频道非广告曲目使用。返回播放列表，客户端应立即停止当前歌曲，并播放新列表。
*/


        private const string playList = "playlist";
        private const string likedSongs = "liked_songs";
        private const string songDetail = "song_detail";
        private const string songUrl = "song_url";

        private string accessToken = "";

        public string AccessToken
        {
            set { accessToken = value; }
        }


        public async Task<PlayList> GetPlayList(int channel)
        {
            var paramSet = new Dictionary<string, string>
            {
                { "channel",channel.ToString() },
                { "mode", "offline" },
                { "kbps", "64"},
                { "max", "30" },
                { "type","n" }
            };
            var requestUri = BuildRequestUri(BaseUrl, playList, paramSet);

            return await SendRequestAsync<PlayList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<PlayList> Like(string sid, int channel)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"sid",sid },
                { "channel",channel.ToString() },
                {"type","r" }
            };
            var requestUri = BuildRequestUri(BaseUrl, playList, paramSet);
            return await SendRequestAsync<PlayList>(requestUri, accessToken, HttpMethod.Get);

        }

        public async Task<PlayList> Unlike(string sid, int channel)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"sid",sid },
                { "channel",channel.ToString() },
                {"type","u" }
            };
            var requestUri = BuildRequestUri(BaseUrl, playList, paramSet);
            return await SendRequestAsync<PlayList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<PlayList> Ban(string sid, int channel)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"sid",sid },
                { "channel",channel.ToString() },
                {"type","b" }
            };
            var requestUri = BuildRequestUri(BaseUrl, playList, paramSet);
            return await SendRequestAsync<PlayList>(requestUri, accessToken, HttpMethod.Get);

        }

        public async Task<PlayList> Skip(string sid, int channel)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"sid",sid },
                { "channel",channel.ToString() },
                {"type","s" }
            };
            var requestUri = BuildRequestUri(BaseUrl, playList, paramSet);
            return await SendRequestAsync<PlayList>(requestUri, accessToken, HttpMethod.Get);
        }

        public async Task<PlayList> NormalEnd(string sid, int channel)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"sid",sid },
                { "channel",channel.ToString() },
                {"type","e" }
            };
            var requestUri = BuildRequestUri(BaseUrl, playList, paramSet);
            return await SendRequestAsync<PlayList>(requestUri, accessToken, HttpMethod.Get);
        }

    }

}
