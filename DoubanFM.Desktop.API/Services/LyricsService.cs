using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubanFM.Desktop.API.Models;
using RestSharp;

namespace DoubanFM.Desktop.API.Services
{
    public class LyricsService : ILyricsService
    {
        public const string BaseUrl = "http://music.douban.com/";
        public async Task<Lyrics> GetLyrics(Song song)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest("api/song/info", Method.GET);
            request.AddParameter("song_id", song.SID);
            var response = await client.ExecuteTaskAsync<SongInfo>(request);
            var lyrics = response.Data.Lyric;
            return new Lyrics(lyrics);
        }
    }
}
