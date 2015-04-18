using DoubanFM.Desktop.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DoubanFM.Desktop.NowPlaying.Design
{
    internal class NowPlayingDesignViewModel
    {
        public NowPlayingDesignViewModel()
        {
            if (Infrastructure.Extension.d.IsInDesignMode)
                LoadDesignTimeData();
            else
                return;
        }

        private void LoadDesignTimeData()
        {
            CurrentSong=new Song
            {
                Artist = "Rogue Wave",
                Company = "Hollywood Records",
                Title = "No Time",
                AlbumTitle = "Iron Man 3: Heroe...",
                Like=true,
                Length=194,
                Picture = "http://img5.douban.com/lpic/s26379626.jpg"
            };

            CurrentChannel = new Channel
            {
                Name = "红心兆赫"
            };

            BackgroundColor = new SolidColorBrush(Colors.DeepSkyBlue);

            CurrentAlbumImage = CurrentSong.Picture;
            
        }

        public Song CurrentSong { get; set; }

        public Channel CurrentChannel { get; set; }

        public Brush BackgroundColor { get; set; }

        public string CurrentAlbumImage { get; set; }
    }
}
