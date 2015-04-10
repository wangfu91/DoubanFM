using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubanFM.Desktop.API.Models;
using RestSharp;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DoubanFM.Desktop.API.Services
{
    public class LyricsService : ILyricsService
    {
        public const string BaseUrl = "http://music.douban.com/";

        public async Task<Lyrics> GetLyrics(string sid)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("api/song/info", Method.GET);
            request.AddParameter("song_id", sid);
            var response = await client.ExecuteTaskAsync(request);
            var lyrics = Serialize<SongInfo>(response).Lyric;
            return new Lyrics(lyrics);
        }

        private T Serialize<T>(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                return JObject.Parse(content).ToObject<T>();
            }

            return default(T);
        }

    }
}
