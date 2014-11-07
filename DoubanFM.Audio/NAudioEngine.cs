using DoubanFM.Data;
using DoubanFM.Service;
using Microsoft.Practices.Prism.Commands;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WPFSoundVisualizationLib;


namespace DoubanFM.Audio
{
    public class NAudioEngine : ISpectrumPlayer, INotifyPropertyChanged, IDisposable
    {

        private static NAudioEngine instance;
        private bool disposed;
        private WaveOut waveOutDevice;
        private WaveStream activeStream;
        private double songLength = 0.0;
        private PlayListService playListService = new PlayListService();
        private SampleAggregator sampleAggregator;
        private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
        private WaveChannel32 inputStream;


        #region Notification Properties

        private bool canPlay;

        public bool CanPlay
        {
            get
            {
                return canPlay;
            }
            set
            {
                if (value != canPlay)
                {
                    canPlay = value;
                    NotifyPropertyChanged("CanPlay");
                    PlayOrPauseCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool canPause;

        public bool CanPause
        {
            get
            {
                return canPause;
            }
            set
            {
                if (value != canPause)
                {
                    canPause = value;
                    NotifyPropertyChanged("CanPause");
                    LikeCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private bool canStop;

        public bool CanStop
        {
            get
            {
                return canStop;
            }
            set
            {
                if (value != canStop)
                {
                    canStop = value;
                    NotifyPropertyChanged("CanStop");
                    StopCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private bool isPlaying;

        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                if (value != isPlaying)
                {
                    isPlaying = value;
                    NotifyPropertyChanged("IsPlaying");
                }
            }
        }

        private bool isLiked;

        public bool IsLiked
        {
            get
            {
                return isLiked;
            }
            set
            {
                if (value != isLiked)
                {
                    isLiked = value;
                    NotifyPropertyChanged("IsLiked");
                }
            }
        }



        private Song currentSong;

        public Song CurrentSong
        {
            get
            {
                return currentSong;
            }
            set
            {
                if (value != currentSong)
                {
                    currentSong = value;
                    NotifyPropertyChanged("CurrentSong");
                }
            }
        }

        private Queue<Song> playList;

        public Queue<Song> PlayList
        {
            get
            {
                return playList;
            }
            set
            {
                if (value != playList)
                {
                    playList = value;
                    NotifyPropertyChanged("PlayList");
                }
            }
        }


        #endregion



        public DelegateCommand PlayOrPauseCommand { get; set; }

        public DelegateCommand LikeCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }

        public DelegateCommand NextCommand { get; set; }

        public DelegateCommand DeleteCommand { get; set; }

        public static NAudioEngine Instance
        {
            get
            {
                if (instance == null)
                    instance = new NAudioEngine();
                return instance;
            }
        }

        private NAudioEngine()
        {
            this.PlayOrPauseCommand = new DelegateCommand(async () =>
                {
                    if (IsPlaying)
                        await Pause();
                    else
                        await Play();
                });
            this.LikeCommand = new DelegateCommand(async () =>
                {
                    if (CurrentSong.Like)
                        await UnLike();
                    else
                        await Like();
                });
            this.StopCommand = new DelegateCommand(async () => await Stop(), () => CanStop);
            this.NextCommand = new DelegateCommand(async () => await PlayNext());
            this.DeleteCommand = new DelegateCommand(async () => await Delete());
            PlayList = new Queue<Song>();
            waveOutDevice = new WaveOut();
            waveOutDevice.PlaybackStopped += waveOutDevice_PlaybackStopped;
        }


        async void waveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            await PlayNext();
        }

        public Task Initialize(string filePath)
        {
            return Task.Run(() =>
                {
                    CloseStream();

                    if (File.Exists(filePath))
                    {
                        activeStream = new AudioFileReader(filePath);
                        sampleAggregator = new SampleAggregator(fftDataSize);
                        waveOutDevice.Init(activeStream);
                        songLength = activeStream.TotalTime.TotalSeconds;
                        CanPlay = true;
                    }
                    else
                    {
                        throw (new FileNotFoundException());
                    }
                });
        }



        public async Task PlayNext()
        {
            if (PlayList.Count < 1)
                await GetPlayList();

            if (waveOutDevice.PlaybackState != PlaybackState.Stopped)
                waveOutDevice.Pause();

            CurrentSong = PlayList.Dequeue();

            var filePath = Path.Combine(Environment.CurrentDirectory, CurrentSong.Title + ".mp3");

            using (var httpClient = new HttpClient())
            {
                var bytes = await httpClient.GetByteArrayAsync(CurrentSong.URL);
                using (var writer = new FileStream(filePath, FileMode.Create))
                {
                    await writer.WriteAsync(bytes, 0, bytes.Length);
                }
            }

            await Instance.Initialize(filePath);
            await Instance.Play();

        }

        private async Task GetPlayList()
        {
            var songList = await playListService.SendRequest("2", "n", "");
            songList.Songs.ForEach(s => this.PlayList.Enqueue(s));
        }

        public Task Play()
        {
            return Task.Run(() =>
                {
                    if (CanPlay)
                    {
                        waveOutDevice.Play();
                        IsPlaying = true;
                        CanPause = true;
                        CanPlay = false;
                        CanStop = true;
                    }
                });
        }


        public Task Pause()
        {
            return Task.Run(() =>
                {
                    if (IsPlaying && CanPause)
                    {
                        waveOutDevice.Pause();
                        IsPlaying = false;
                        CanPause = false;
                        CanPlay = true;
                    }
                });

        }

        public Task Like()
        {
            return Task.Run(() =>
            {
                var result = playListService.SendRequest("", "r", CurrentSong.SID);

            });
        }

        public Task UnLike()
        {
            return Task.Run(() =>
            {
                var result = playListService.SendRequest("", "u", CurrentSong.SID);

            });
        }

        public Task Delete()
        {
            return Task.Run(() =>
                {
                    var result = playListService.SendRequest("", "s", CurrentSong.SSID);
                });
        }


        public Task Stop()
        {
            return Task.Run(() =>
                {
                    if (waveOutDevice != null)
                    {
                        waveOutDevice.Stop();
                    }

                    IsPlaying = false;
                    CanStop = false;
                    CanPlay = true;
                    CanPause = false;
                });
        }


        private void CloseStream()
        {
            if (activeStream != null)
            {
                activeStream.Dispose();
                activeStream = null;
            }
        }

        private void StopAndCloseWave()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    CloseStream();
                    StopAndCloseWave();
                }
                disposed = true;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion

        #region ISpectrumPlayer

        public bool GetFFTData(float[] fftDataBuffer)
        {
            sampleAggregator.GetFFTResults(fftDataBuffer);
            return IsPlaying;
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            double maxFrequency;
            if (activeStream != null)
                maxFrequency = activeStream.WaveFormat.SampleRate / 2.0d;
            else
                maxFrequency = 22050; // Assume a default 44.1 kHz sample rate.
            return (int)((frequency / maxFrequency) * (fftDataSize / 2));

        }

        #endregion
    }
}
