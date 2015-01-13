using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Models
{
    public class SongInfo
    {
        [JsonProperty("artist_region")]
        public string ArtistRegion { get; set; }

        [JsonProperty("album_stars")]
        public int AlbumStars { get; set; }

        [JsonProperty("album_rate")]
        public string AlbumRate { get; set; }

        [JsonProperty("artist_name")]
        public string ArtistName { get; set; }

        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }

        [JsonProperty("lyric")]
        public string Lyric { get; set; }

        [JsonProperty("albums")]
        public List<Album> Albums { get; set; }

        [JsonProperty("artist_birth")]
        public string ArtistBirth { get; set; }

        [JsonProperty("subject_id")]
        public string SubjectId { get; set; }

        [JsonProperty("album_intro")]
        public string AlbumIntro { get; set; }

        [JsonProperty("artist_intro")]
        public string ArtistIntro { get; set; }

        [JsonProperty("artist_id")]
        public string ArtistId { get; set; }

        [JsonProperty("artist_genre")]
        public string ArtistGenre { get; set; }
    }

    public class Photo
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }


}
