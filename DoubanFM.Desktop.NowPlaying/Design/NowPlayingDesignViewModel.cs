using DoubanFM.Desktop.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                Artist="张靓颖",
                Company="少城时代",
                Title="朝思暮想",
                AlbumTitle="我相信",
                Like=true,
                Length=257,
                Picture = "http://img3.douban.com/lpic/s4165622.jpg"
            };
        }

        public Song CurrentSong { get; set; }
    }
}
