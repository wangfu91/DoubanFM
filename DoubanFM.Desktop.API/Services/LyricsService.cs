using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubanFM.Desktop.API.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace DoubanFM.Desktop.API.Services
{
    public class LyricsService : ServiceBase, ILyricsService
    {
        private const string lyric = "lyric";

        public async Task<Lyrics> GetLyrics(string sid, string ssid)
        {
            var paramSet = new Dictionary<string, string>
            {
                {"sid",sid },
                {"ssid", ssid }
            };
            var requestUri = BuildRequestUri(BaseUrl, lyric, paramSet);
            return await SendRequestAsync<Lyrics>(requestUri, "", HttpMethod.Get);
        }
    }
}
