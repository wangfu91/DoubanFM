using DoubanFM.Service;
using System.Windows;

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
            this.DataContext = new ViewModels.MainPageViewModel();
            //Load();

            //Login();
        }


        private async void Load()
        {
            var channelService = new ChannelService();
            //var channels=await channelService.GetChannels(1,"n");
            //var count = channels.Count;

            var playListService = new PlayListService();
            var playList = await playListService.GetPlayList(1, "n", "");
            var songs = playList.Song;

        }



        private async void Login()
        {
            var service = new LoginService();
            var logonInfo = await service.Login("wangfu91@hotmail.com", "wf19912012");
        }
    }

}
