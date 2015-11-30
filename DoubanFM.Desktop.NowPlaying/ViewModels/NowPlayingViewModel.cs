using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Audio;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private IEventAggregator _eventAggregator;
        private bool _isLoggedIn;
        private BitmapImage _currentAlbumImage;
        private Brush _backgroundColor;

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
                        HandleCurrentSongChange().ConfigureAwait(false);
                    }
                }
            }
        }

        public BitmapImage CurrentAlbumImage
        {
            get { return _currentAlbumImage; }
            set
            {
                OnPropertyChanged(() => this.CurrentAlbumImage);
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

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    OnPropertyChanged(() => this.BackgroundColor);
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
        private async Task HandleCurrentSongChange()
        {
            LoadAlbumImage();
            await Player.OpenUrl(_currentSong.URL);
            if (Player.PlayCommand.CanExecute(null))
                Player.PlayCommand.Execute(null);

            await GetLyrics();
        }

        private void LoadAlbumImage()
        {
            //Always initialize BitmapImage from UI thread, so that there won't be any cross thread issue,
            //also DownloadCompleted event can be fired properly even if invoke this method in an async way.
            Application.Current.Dispatcher.Invoke(() =>
                {
                    _currentAlbumImage = new BitmapImage(new Uri(CurrentSong.Picture));
                    _currentAlbumImage.DownloadCompleted += _currentAlbumImage_DownloadCompleted;
                });
        }

        private async Task GetSongs()
        {
            var result = await _songService.GetPlayList(CurrentChannel.Id);
            SetPlayList(result);
            if (PlayList.Count > 0)
            {
                CurrentSong = PlayList.Dequeue();
            }
        }

        private async Task GetLyrics()
        {
            var _lyrics = await _lyricsService.GetLyrics(_currentSong.SID, _currentSong.SSID);
        }

        private void SetPlayList(PlayList result)
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
            this._songService = new SongService();
            if (result != null)
            {
                this.IsLoggedIn = true;
                _songService.AccessToken = result.AccessToken;
            }
            else
            {
                this.IsLoggedIn = false;
            }
        }

        private async Task PlayNext()
        {
            if (PlayList.Count > 0)
            {
                CurrentSong = PlayList.Dequeue();
                SetPlayList(await _songService.Skip(CurrentSong.SID, CurrentChannel.Id));
            }
            else
            {
                SetPlayList(await _songService.Skip(CurrentSong.SID, CurrentChannel.Id));
                CurrentSong = PlayList.Dequeue();
            }

        }

        private async Task Ban()
        {
            if (PlayList.Count > 0)
            {
                CurrentSong = PlayList.Dequeue();
                SetPlayList(await _songService.Ban(CurrentSong.SID, CurrentChannel.Id));
            }
            else
            {
                SetPlayList(await _songService.Ban(CurrentSong.SID, CurrentChannel.Id));
                CurrentSong = PlayList.Dequeue();
            }
        }


        private async Task Like()
        {
            if (CurrentSong.Like)
            {
                var result = await _songService.Unlike(CurrentSong.SID, CurrentChannel.Id);
                if (result.R == 0)
                {
                    CurrentSong.Like = false;
                    SetPlayList(result);
                }
            }
            else
            {
                var result = await _songService.Like(CurrentSong.SID, CurrentChannel.Id);
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
            await PlayNext();
        }


        private async void _timer_Tick(object sender, EventArgs e)
        {

        }

        private void _currentAlbumImage_DownloadCompleted(object sender, EventArgs e)
        {
            this.CurrentAlbumImage = _currentAlbumImage;
            var color = ColorFunctions.GetImageColor(_currentAlbumImage);
            this.BackgroundColor = new SolidColorBrush(color);
            _eventAggregator.GetEvent<SwitchBackgroudColorEvent>().Publish(color);
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.CurrentAlbumImage = null;
            this.PlayList = null;
            this.CurrentSong = null;
            this.CurrentChannel = null;
            this.CurrentLyrics = null;
            this._playEngine.TrackEnded -= playEngine_TrackeEnded;
            this._timer.Tick -= _timer_Tick;
            this._currentAlbumImage.DownloadCompleted -= _currentAlbumImage_DownloadCompleted;
        }

        #endregion


    }
}
