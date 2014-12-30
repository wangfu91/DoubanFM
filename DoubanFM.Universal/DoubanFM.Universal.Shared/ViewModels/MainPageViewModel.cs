using DoubanFM.Universal.APIs.Models;
using DoubanFM.Universal.APIs.Services;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace DoubanFM.Universal.ViewModels
{
    public class MainPageViewModel : ViewModel
    {

        private BitmapImage _albumImage;
        private Channel _currentChannel;
        private Song _currentSong;
        private List<Channel> _channels;
        private Queue<Song> _playList;

        private ILoginService _loginService;
        private IUserService _userService;
        private IChannelService _channelService;
        private ISongService _songService;


        public BitmapImage AlbumImage
        {
            get { return _albumImage; }
            set
            {
                if (value != _albumImage)
                {
                    _albumImage = value;
                    OnPropertyChanged(() => this.AlbumImage);
                }
            }
        }

        public Channel CurrentChannel
        {
            get { return _currentChannel; }
            set
            {
                if(value!=_currentChannel)
                {
                    _currentChannel = value;
                    OnPropertyChanged(() => this.CurrentChannel);
                }
            }
        }

        public Song CurrentSong
        {
            get { return _currentSong; }
            set
            {
                if (value != _currentSong)
                {
                    _currentSong = value;
                    OnPropertyChanged(() => this.CurrentSong);
                }
            }
        }

        public List<Channel> Channels
        {
            get { return _channels; }
            set
            {
                if (value != _channels)
                {
                    _channels = value;
                    OnPropertyChanged(() => this.Channels);
                }
            }
        }

        public Queue<Song> PlayList
        {
            get { return _playList; }
            set
            {
                if(value!=_playList)
                {
                    _playList = value;
                    OnPropertyChanged(() => this.PlayList);
                }
            }
        }

        public MainPageViewModel(
            ILoginService loginService,
            IUserService userService,
            IChannelService channelService,
            ISongService songService)
        {
            _loginService = loginService;
            _userService = userService;
            _channelService = channelService;
            _songService = songService;
            
        }

        private async void Initialize()
        {
            await LoadChannels();
            await LoadPlayList();
        }

        private async Task LoadChannels()
        {
            var channelList = await _channelService.GetChannels();
            this.Channels= channelList.Channels ?? new List<Channel>();
            this.CurrentChannel = Channels.FirstOrDefault();
        }

        private async Task LoadPlayList()
        {
            var channelId = CurrentChannel.ChannelId;
            var songResult = await _songService.GetSongs(channelId);
            var songs= songResult.Songs ?? new List<Song>();

            foreach (var song in songs)
            {
                this.PlayList.Enqueue(song);
            }
        }

    }
}
