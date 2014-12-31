using Microsoft.Practices.Prism.Commands;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.WaveOutputs;
using SoundVisualizationLib.Universal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace DoubanFM.Universal.Player
{
    public class Player : IPlayer
    {
        #region Fields
        private static Player instance;
        private readonly DispatcherTimer positionTimer = new DispatcherTimer();
        private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
        private bool disposed;
        private bool canPlay;
        private bool canPause;
        private bool canStop;
        private bool isPlaying;
        private bool inChannelTimerUpdate;
        private double channelLength;
        private double channelPosition;
        private bool inChannelSet;
        private IWavePlayer wavePlayer;
        private WaveStream waveStream;
        private IRandomAccessStream fileStream;
        private WaveChannel32 inputStream;
        private SampleAggregator sampleAggregator;
        private BufferedWaveProvider bufferedWaveProvider;
        private volatile StreamingPlaybackState playbackState;
        private volatile bool fullyDownloaded;
        private HttpWebRequest webRequest;
        #endregion

        #region events
        public event EventHandler TrackEnded;
        #endregion

        #region Singleton 
        public static Player Instance
        {
            get
            {
                return instance ?? new Player();
            }
        }
        #endregion

        #region Constructor
        private Player()
        {
            positionTimer.Interval = TimeSpan.FromMilliseconds(50);
            positionTimer.Tick += positionTimer_Tick;

            wavePlayer.PlaybackStopped += waveOutDevice_PlaybackStopped;

            this.PlayPauseCommand = new DelegateCommand(() =>
                {
                    if (IsPlaying)
                        Pause();
                    else
                        Play();
                });

            this.StopCommand = new DelegateCommand(() => Stop(), () => CanStop);

        }

        #endregion

        #region Notification Properties
        public double ChannelLength
        {
            get
            {
                return channelLength;
            }
            set
            {
                if (value != channelLength)
                {
                    channelLength = value;
                    NotifyPropertyChanged("ChannelLength");
                }
            }
        }

        public double ChannelPosition
        {
            get { return channelPosition; }
            set
            {
                if (!inChannelSet)
                {
                    inChannelSet = true; // Avoid recursion
                    double oldValue = channelPosition;
                    double position = Math.Max(0, Math.Min(value, ChannelLength));
                    if (!inChannelTimerUpdate && waveStream != null)
                        waveStream.Position = (long)((position / waveStream.TotalTime.TotalSeconds) * waveStream.Length);
                    channelPosition = position;
                    if (oldValue != channelPosition)
                        NotifyPropertyChanged("ChannelPosition");
                    inChannelSet = false;
                }
            }
        }

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
                }
            }
        }


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
                }
            }
        }



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
                }
            }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            protected set
            {
                isPlaying = value;
                if (value != isPlaying)
                {
                    isPlaying = value;
                    NotifyPropertyChanged("IsPlaying");
                    if (isPlaying)
                        positionTimer.Start();
                }
            }
        }

        #endregion

        #region Commands
        public ICommand PlayPauseCommand { get; set; }

        public ICommand PlayNextCommand { get; set; }

        public ICommand StopCommand { get; set; }
        #endregion

        #region Public Methods

        public async void OpenFile(string path)
        {

            Stop();

            if (waveStream != null)
            {
                ChannelPosition = 0;
            }

            StopAndCloseStream();

            var file = await StorageFile.GetFileFromPathAsync(path);
            if (file == null) return;
            fileStream = await file.OpenReadAsync();
            if (fileStream == null) return;

            try
            {
                wavePlayer = new WasapiOutRT(AudioClientShareMode.Shared, 200);

                waveStream = new MediaFoundationReaderRT(fileStream);
                inputStream = new WaveChannel32(waveStream);
                sampleAggregator = new SampleAggregator(fftDataSize);
                await wavePlayer.Init(inputStream);
                ChannelLength = inputStream.TotalTime.TotalSeconds;
                CanPlay = true;
            }
            catch (Exception ex)
            {
                waveStream = null;
                CanPlay = false;

                Debug.WriteLine("Open file:{0} failed !", path);
                Debug.WriteLine("Exception:{0}", ex.Message);
            }

        }

        public void OpenUrl(string url)
        {

        }
        public void Play()
        {
            if (CanPlay)
            {
                wavePlayer.Play();
                IsPlaying = true;
                CanPause = true;
                CanPlay = false;
                CanStop = true;
            }
        }


        public void Pause()
        {
            if (IsPlaying && CanPause)
            {
                wavePlayer.Pause();
                IsPlaying = false;
                CanPause = false;
                CanPlay = true;
            }
        }


        public void Stop()
        {

            if (wavePlayer != null)
            {
                wavePlayer.Stop();
            }

            IsPlaying = false;
            CanStop = false;
            CanPlay = true;
            CanPause = false;
        }

        #endregion

        #region Private Methods
        private void waveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            OnTrackEnded();
        }

        private void StopAndCloseStream()
        {
            if (wavePlayer != null)
            {
                wavePlayer.Stop();
            }
            if (waveStream != null)
            {
                inputStream.Dispose();
                inputStream = null;
                waveStream.Dispose();
                waveStream = null;
            }
            if (wavePlayer != null)
            {
                wavePlayer.Dispose();
                wavePlayer = null;
            }
        }

        #endregion

        #region Event Handlers
        private void OnTrackEnded()
        {
            if (TrackEnded != null)
                TrackEnded(this, null);
        }

        void positionTimer_Tick(object sender, object e)
        {
            inChannelTimerUpdate = true;
            ChannelPosition = ((double)waveStream.Position / (double)waveStream.Length) * waveStream.TotalTime.TotalSeconds;
            inChannelTimerUpdate = false;
        }
        #endregion

        #region ISpectrumPlayer
        public bool GetFFTData(float[] fftDataBuffer)
        {
            sampleAggregator.GetFFTResults(fftDataBuffer);
            return isPlaying;
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            double maxFrequency;
            if (waveStream != null)
                maxFrequency = waveStream.WaveFormat.SampleRate / 2.0d;
            else
                maxFrequency = 22050; // Assume a default 44.1 kHz sample rate.
            return (int)((frequency / maxFrequency) * (fftDataSize / 2));
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
                    StopAndCloseStream();
                }
                disposed = true;
            }
        }

        #endregion

    }

}

