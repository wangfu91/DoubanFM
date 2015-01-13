﻿using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Audio;
using DoubanFM.Desktop.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DoubanFM.Desktop.Core.ViewModels
{
    public class PlayerUIViewModel : ViewModelBase
    {
        private LoginService _loginService;
        private UserService _userService;
        private SongService _songService;
        private ChannelService _channelService;
        private IAudioEngine _playEngine;
        private ChannelList _channelList;
        private Song _currentSong;
        private Channel _currentChannel;
        private Queue<Song> _playList;

        public IAudioEngine Player
        {
            get { return _playEngine; }
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
                    if (_currentSong != null)
                    {
                        Player.OpenUrl(_currentSong.URL);
                        Player.Play();
                    }
                }
            }
        }

        public Channel CurrentChannel
        {
            get { return _currentChannel; }
            set
            {
                if (value != _currentChannel)
                {
                    _currentChannel = value;
                    OnPropertyChanged(() => this.CurrentChannel);
                }
            }

        }

        public Queue<Song> PlayList
        {
            get { return _playList; }
            set
            {
                if (value != _playList)
                {
                    _playList = value;
                    OnPropertyChanged(() => this.PlayList);
                }
            }
        }

        public ChannelList ChannelList
        {
            get { return _channelList; }
            set
            {
                if (value != _channelList)
                {
                    _channelList = value;
                    OnPropertyChanged(() => this.ChannelList);
                }
            }
        }


        public ICommand PlayNextCommand { get; set; }

        public ICommand LikeCommand { get; set; }

        public ICommand BanCommand { get; set; }




        public PlayerUIViewModel(IAudioEngine playEngine)
        {
            this._playEngine = playEngine;
            PlayList = new Queue<Song>();
            playEngine.TrackEnded += playEngine_TrackeEnded;
            Initialize();
            PlayNextCommand = new DelegateCommand(async () =>
                {
                    if (PlayList.Count > 0)
                    {
                        CurrentSong = PlayList.Dequeue();
                        SetPlayList(await _songService.Skip(CurrentSong.SID, CurrentChannel.ChannelId));
                    }
                    else
                    {
                        SetPlayList(await _songService.Skip(CurrentSong.SID, CurrentChannel.ChannelId));
                        CurrentSong = PlayList.Dequeue();
                    }
                });

            LikeCommand = new DelegateCommand(async () =>
            {
                if (CurrentSong.Like)
                {
                    var result = await _songService.Unlike(CurrentSong.SID, CurrentChannel.ChannelId);
                    if (result.R == 0)
                    {
                        CurrentSong.Like = false;
                        SetPlayList(result);
                    }
                }
                else
                {
                    var result = await _songService.Like(CurrentSong.SID, CurrentChannel.ChannelId);
                    if (result.R == 0)
                    {
                        CurrentSong.Like = true;
                        SetPlayList(result);
                    }
                }

            });

            BanCommand = new DelegateCommand(async () =>
            {
                if (PlayList.Count > 0)
                {
                    CurrentSong = PlayList.Dequeue();
                    SetPlayList(await _songService.Ban(CurrentSong.SID, CurrentChannel.ChannelId));
                }
                else
                {
                    SetPlayList(await _songService.Ban(CurrentSong.SID, CurrentChannel.ChannelId));
                    CurrentSong = PlayList.Dequeue();
                }
            });
        }

        private async Task Login(string email, string password)
        {
            _loginService = new LoginService();
            var loginResult = await _loginService.LoginWithEmail(email, password);
            var userSvcParams = new UserParams(loginResult);
            _channelService = new ChannelService(loginResult);
            _songService = new SongService(loginResult);
        }


        private async void Initialize()
        {
            await Login("wangfu91@hotmail.com", "wf19912012");

            ChannelList = await _channelService.GetChannels();
            if (ChannelList != null)
            {
                CurrentChannel = ChannelList.Channels.First();
                CurrentChannel.ChannelId = "-3";
                await GetSongs();
            }
        }

        private async Task GetSongs()
        {
            var result = await _songService.GetSongs(CurrentChannel.ChannelId);
            SetPlayList(result);
            CurrentSong = PlayList.Dequeue();
        }

        private void SetPlayList(SongResult result)
        {
            if (result != null && result.Songs.Count > 0)
            {
                PlayList.Clear();
                result.Songs.ForEach(s => PlayList.Enqueue(s));
            }
        }


        private async void playEngine_TrackeEnded(object sender, EventArgs e)
        {

            if (PlayList.Count > 0)
            {
                CurrentSong = PlayList.Dequeue();
                var result = await _songService.NormalEnd(CurrentSong.SID, CurrentChannel.ChannelId);
                SetPlayList(result);
            }
            else
            {
                var result = await _songService.NormalEnd(CurrentSong.SID, CurrentChannel.ChannelId);
                SetPlayList(result);
                CurrentSong = PlayList.Dequeue();
            }
        }

    }

}