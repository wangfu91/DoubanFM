using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Audio;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace DoubanFM.Desktop.NowPlaying.ViewModels
{
    public class NowPlayingViewModel : ViewModelBase
    {

        #region Fields
        private ISongService _songService;
        private ILyricsService _lyricsService;
        private IAudioEngine _playEngine;
        private Song _currentSong;
        private string _currentLyrics;
        private Channel _currentChannel;
        private Queue<Song> _playList;
        private DispatcherTimer _timer = new DispatcherTimer();
        private LyricController _lyricsController;
        private IEventAggregator _eventAggregator;
        private bool _isLoggedIn;
        #endregion

        #region Constructor
        public NowPlayingViewModel(
            IEventAggregator eventAggregator,
            IAudioEngine playEngine,
            ISongService songService,
            ILyricsService lyricService)
        {
            this._eventAggregator = eventAggregator;
            this._playEngine = playEngine;
            this._songService = songService;
            this._lyricsService = lyricService;

            PlayList = new Queue<Song>();

            var switchChannelEvent = _eventAggregator.GetEvent<SwitchChannelEvent>();
            switchChannelEvent.Subscribe(async c => await HandleChannelChange(c));

            var userStateChnagedEvent = _eventAggregator.GetEvent<UserStateChangedEvent>();
            userStateChnagedEvent.Subscribe(HandleUserStateChange);

            playEngine.TrackEnded += playEngine_TrackeEnded;

            PlayNextCommand = new DelegateCommand(async () => await PlayNext());

            LikeCommand = new DelegateCommand(async () => await Like(), () => this.IsLoggedIn);

            BanCommand = new DelegateCommand(async () => await Ban(), () => this.IsLoggedIn);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.4);
            _timer.Tick += _timer_Tick;
            _timer.Start();

        }

        #endregion

        #region Properties

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
                        ChangeSong().ConfigureAwait(false);
                    }
                }
            }
        }

        public string CurrentLyrics
        {
            get { return _currentLyrics; }
            set
            {
                if (value != _currentLyrics)
                {
                    _currentLyrics = value;
                    OnPropertyChanged(() => this.CurrentLyrics);
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

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            private set
            {
                if (value != _isLoggedIn)
                {
                    _isLoggedIn = value;
                    OnPropertyChanged(() => this.IsLoggedIn);

                    this.BanCommand.RaiseCanExecuteChanged();
                    this.LikeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commands
        public DelegateCommand PlayNextCommand { get; set; }

        public DelegateCommand LikeCommand { get; set; }

        public DelegateCommand BanCommand { get; set; }

        #endregion

        #region Private Methods
        private async Task ChangeSong()
        {
            await GetLyrics();
            await Player.OpenUrl(_currentSong.URL);
            if (Player.PlayCommand.CanExecute(null))
                Player.PlayCommand.Execute(null);
        }

        private async Task GetSongs()
        {
            var result = await _songService.GetSongs(CurrentChannel.ChannelId);
            SetPlayList(result);
            CurrentSong = PlayList.Dequeue();
        }

        private async Task GetLyrics()
        {
            var _lyrics = await _lyricsService.GetLyrics(_currentSong.SID);
            _lyricsController = null;
            if (!string.IsNullOrEmpty(_lyrics.LrcCode))
            {
                _lyricsController = new LyricController(_lyrics);
            }
        }

        private void SetPlayList(SongResult result)
        {
            if (result != null && result.Songs.Count > 0)
            {
                PlayList.Clear();
                result.Songs.ForEach(s => PlayList.Enqueue(s));
            }
        }

        private async Task HandleChannelChange(Channel selectedChannel)
        {
            CurrentChannel = selectedChannel;
            await GetSongs();
        }

        private void HandleUserStateChange(LoginResult result)
        {
            if (result != null)
            {
                this._songService = new SongService(result);
                this.IsLoggedIn = true;
            }
            else
            {
                this._songService = new SongService();
                this.IsLoggedIn = false;
            }
        }

        private async Task PlayNext()
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

        }

        private async Task Ban()
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
        }


        private async Task Like()
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
        }

        #endregion

        #region Event Handlers
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

        private async void _timer_Tick(object sender, EventArgs e)
        {
            if (_lyricsController != null)
            {
                _lyricsController.CurrentTime = TimeSpan.FromSeconds(Player.ChannelPosition);
                await _lyricsController.RefreshAsync();
                CurrentLyrics = _lyricsController.CurrentLyrics;
            }
        }

        #endregion

    }
}
