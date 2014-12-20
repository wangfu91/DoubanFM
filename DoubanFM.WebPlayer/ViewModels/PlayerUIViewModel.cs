using DoubanFM.Audio;
using DoubanFM.Common;
using DoubanFM.Data;
using DoubanFM.Service;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DoubanFM.WebPlayer.ViewModels
{
    public class PlayerUIViewModel : ViewModelBase
    {
        private UserService userService;
        private SongService songService;
        private ChannelService channelService;
        private IAudioEngine playEngine;
        private ChannelList channelList;
        private Song currentSong;
        private Channel currentChannel;
        private Queue<Song> playList;

        public IAudioEngine Player
        {
            get { return playEngine; }
        }

        public Song CurrentSong
        {
            get { return currentSong; }
            set
            {
                if (value != currentSong)
                {
                    currentSong = value;
                    OnPropertyChanged(() => this.CurrentSong);
                    if (currentSong != null)
                    {
                        Player.OpenUrl(currentSong.URL);
                        Player.Play();
                    }
                }
            }
        }

        public Channel CurrentChannel
        {
            get { return currentChannel; }
            set
            {
                if (value != currentChannel)
                {
                    currentChannel = value;
                    OnPropertyChanged(() => this.CurrentChannel);
                }
            }

        }

        public Queue<Song> PlayList
        {
            get { return playList; }
            set
            {
                if (value != playList)
                {
                    playList = value;
                    OnPropertyChanged(() => this.PlayList);
                }
            }
        }

        public ChannelList ChannelList
        {
            get { return channelList; }
            set
            {
                if (value != channelList)
                {
                    channelList = value;
                    OnPropertyChanged(() => this.ChannelList);
                }
            }
        }


        public ICommand PlayNextCommand { get; set; }

        public ICommand LikeCommand { get; set; }

        public ICommand BanCommand { get; set; }




        public PlayerUIViewModel(IAudioEngine playEngine)
        {
            this.playEngine = playEngine;
            PlayList = new Queue<Song>();
            playEngine.TrackEnded += playEngine_TrackeEnded;
            Initialize();
            PlayNextCommand = new DelegateCommand(async () =>
                {
                    SetPlayList(await songService.Skip(CurrentSong.SID, CurrentChannel.ChannelId));
                });

            LikeCommand = new DelegateCommand(async () =>
            {
                if (CurrentSong.Like)
                {
                    var result=await songService.Unlike(CurrentSong.SID, CurrentChannel.ChannelId);
                    if (result.R == 0)
                        CurrentSong.Like = false;
                }
                else
                {
                    var result=await songService.Like(CurrentSong.SID, CurrentChannel.ChannelId);
                    if (result.R == 0)
                        CurrentSong.Like = true;
                }

            });

            BanCommand = new DelegateCommand(async () =>
            {
                SetPlayList(await songService.Ban(CurrentSong.SID, CurrentChannel.ChannelId));
            });
        }

        private async Task Login(string email, string password)
        {
            userService = new UserService();
            var loginResult = await userService.LoginWithEmail(email, password);
            var userSvcParams = new UserSvcParams(loginResult);
            channelService = new ChannelService(loginResult);
            songService = new SongService(loginResult);
        }


        private async void Initialize()
        {
            await Login("wangfu91@hotmail.com", "wf19912012");

            ChannelList = await channelService.GetChannels();
            if (ChannelList != null)
            {
                CurrentChannel = ChannelList.Channels.First();
                await GetSongs();
            }
        }

        private async Task GetSongs()
        {
            var result = await channelService.GetSongs(CurrentChannel.ChannelId);
            SetPlayList(result);
        }

        private void SetPlayList(SongResult result)
        {
            if (result != null && result.Songs.Count > 0)
            {
                PlayList.Clear();
                result.Songs.Reverse();
                result.Songs.ForEach(s => PlayList.Enqueue(s));
                CurrentSong = PlayList.Dequeue();
            }
        }


        private async void playEngine_TrackeEnded(object sender, EventArgs e)
        {
            var result = await songService.NormalEnd(CurrentSong.SID, CurrentChannel.ChannelId);
            SetPlayList(result);
        }

    }

}
