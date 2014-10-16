using DoubanFM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoubanFM.Desktop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }


        private  async void Load()
        {
            var channelService = new ChannelService();
            //var channels=await channelService.GetChannels(1,"n");
            //var count = channels.Count;

            var playListService=new PlayListService();
            var playList = await playListService.GetPlayList(1, "n", "");
            var songs = playList.Song;
            
        }
    }
}
