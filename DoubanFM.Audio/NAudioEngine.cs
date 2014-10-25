using Microsoft.Practices.Prism.Commands;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;


namespace DoubanFM.Audio
{
    public class NAudioEngine : INotifyPropertyChanged, IDisposable
    {

        private static NAudioEngine instance;
        private bool disposed;
        private WaveOut waveOutDevice;
        private WaveStream fileStream;
        private double songLength = 0.0;

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
                    PlayCommand.RaiseCanExecuteChanged();
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
                    NotifyPropertyChanged("canPause");
                    PauseCommand.RaiseCanExecuteChanged();
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
                    NotifyPropertyChanged("canStop");
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
                    NotifyPropertyChanged("isPlaying");
                }
            }
        }

        #endregion

        public DelegateCommand PlayCommand { get; set; }

        public DelegateCommand PauseCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }

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
            this.PlayCommand = new DelegateCommand(async () => await Play(), () => CanPlay);
            this.PauseCommand = new DelegateCommand(async () => await Pause(), () => CanPause);
            this.StopCommand = new DelegateCommand(async () => await Stop(), () => CanStop);
        }

        public Task Initialize(string filePath)
        {
            return Task.Run(() =>
                {
                    StopAndCloseStream();

                    if (File.Exists(filePath))
                    {
                        waveOutDevice = new WaveOut();
                        fileStream = new AudioFileReader(filePath);
                        waveOutDevice.Init(fileStream);
                        songLength = fileStream.TotalTime.TotalSeconds;
                        CanPlay = true;
                    }
                });
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


        private void StopAndCloseStream()
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
            if (fileStream != null)
            {
                fileStream.Dispose();
                fileStream = null;
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
