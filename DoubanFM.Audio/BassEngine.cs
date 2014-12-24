using DoubanFM.Data;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Un4seen.Bass;
using WPFSoundVisualizationLib;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DoubanFM.Audio
{
    public class BassEngine : IAudioEngine
    {
        #region Fileds
        private static BassEngine instance;
        private bool disposed;
        private int fileStreamhandle;
        private int activeStreamHandle;
        private double channelLength;
        private bool canPlay;
        private bool canPause;
        private bool canStop;
        private bool isPlaying;
        private bool isMuted;
        private double volume = 1.0;//default volume is 100%
        private double currentChannelPosition;
        private bool inChannelSet;
        private bool inChannelTimerUpdate;
        private int sampleFrequency = 44100; //44.1KHZ
        /// <summary>
        /// 保存正在打开的文件的地址，当短时间内多次打开网络文件时，这个字段保存最后一次打开的文件，可以使其他打开文件的操作失效
        /// </summary>
        private string openningStream = null;

        private readonly SYNCPROC endTrackSyncProc;
        private readonly DispatcherTimer positionTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
        private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
        private readonly int maxFFT = (int)(BASSData.BASS_DATA_AVAILABLE | BASSData.BASS_DATA_FFT2048);

        #endregion

        #region events
        public event EventHandler TrackEnded;
        #endregion

        #region Singleton
        public static BassEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BassEngine();
                }
                return instance;
            }
        }
        #endregion

        #region Notification Properties
        public int FileStreamHandle
        {
            get
            {
                return fileStreamhandle;
            }
            set
            {
                if (value != fileStreamhandle)
                {
                    fileStreamhandle = value;
                    NotifyPropertyChanged("FileStreamHandle");
                }
            }
        }


        public int ActiveStreamHandle
        {
            get
            {
                return activeStreamHandle;
            }
            set
            {
                if (value != activeStreamHandle)
                {
                    activeStreamHandle = value;
                    if (activeStreamHandle != 0)
                    {
                        SetVolume();
                    }
                    NotifyPropertyChanged("ActiveStreamHandle");
                }
            }
        }


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
            get
            {
                return currentChannelPosition;
            }
            set
            {
                if (!inChannelSet)
                {
                    inChannelSet = true; //Avoid recursion
                    var oldValue = currentChannelPosition;
                    var position = Math.Max(0, Math.Min(value, channelLength));
                    if (!inChannelTimerUpdate)
                    {
                        Bass.BASS_ChannelSetPosition(ActiveStreamHandle, Bass.BASS_ChannelSeconds2Bytes(ActiveStreamHandle, position));
                    }
                    currentChannelPosition = position;
                    if (value != oldValue)
                    {
                        NotifyPropertyChanged("ChannelPosition");
                    }
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
                    //PlayOrPauseCommand.RaiseCanExecuteChanged();
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
                    //StopCommand.RaiseCanExecuteChanged();
                }
            }
        }



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
                    positionTimer.IsEnabled = value;
                    NotifyPropertyChanged("IsPlaying");
                }
            }
        }


        public bool IsMuted
        {
            get { return isMuted; }
            set
            {
                if (value != isMuted)
                {
                    isMuted = value;
                    SetVolume();
                    NotifyPropertyChanged("IsMute");
                }
            }
        }

        public double Volume
        {
            get { return volume; }
            set
            {
                value = Math.Max(0, Math.Min(1, value));
                if (value != volume)
                {
                    volume = value;
                    SetVolume();
                    NotifyPropertyChanged("Volume");
                }
            }
        }

        #endregion

        #region ICommands
        public ICommand PlayPauseCommand { get; set; }

        public ICommand StopCommand { get; set; }

        public ICommand PlayNextCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        #endregion

        #region  Constructor

        private BassEngine()
        {
            this.PlayPauseCommand = new DelegateCommand(() =>
            {
                if (IsPlaying)
                    Pause();
                else
                    Play();
            });

            this.StopCommand = new DelegateCommand(() => Stop(), () => CanStop);

            //注册Bass.Net，不注册就会弹出一个启动画面
            BassNet.Registration("yk000123@sina.com", "2X34201017282922");
            Initialize();
            endTrackSyncProc = (handle, channel, data, user) =>
                {
                    OnTrackEnded();
                };
        }

        #endregion

        #region Public Methods
        public void Stop()
        {
            if (ActiveStreamHandle != 0)
            {
                Bass.BASS_ChannelStop(ActiveStreamHandle);
                Bass.BASS_ChannelSetPosition(ActiveStreamHandle, ChannelPosition);
            }
            IsPlaying = false;
            CanStop = false;
            CanPlay = true;
            CanPause = false;
        }

        public void Pause()
        {
            if (IsPlaying && CanPause)
            {
                Bass.BASS_ChannelPause(ActiveStreamHandle);
                IsPlaying = false;
                CanPlay = true;
                CanPause = false;
            }
        }

        public void Play()
        {
            //if (CanPlay)
            //{
            PlayCurrentStream();
            IsPlaying = true;
            CanPause = true;
            CanPlay = false;
            CanStop = true;
            //}
        }

        public void OpenFile(string path)
        {
            Stop();
            if (ActiveStreamHandle != 0)
            {
                ChannelPosition = 0;
                Bass.BASS_StreamFree(ActiveStreamHandle);
            }

            if (File.Exists(path))
            {
                FileStreamHandle = ActiveStreamHandle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);
                if (ActiveStreamHandle != 0)
                {

                    ChannelLength = Bass.BASS_ChannelBytes2Seconds(FileStreamHandle, Bass.BASS_ChannelGetLength(FileStreamHandle, 0));

                    //Obtain the sample rate of the sstream
                    var info = new BASS_CHANNELINFO();
                    Bass.BASS_ChannelGetInfo(ActiveStreamHandle, info);
                    sampleFrequency = info.freq;

                    // Set the stream to call Stop() when it ends.
                    int syncHandle = Bass.BASS_ChannelSetSync(ActiveStreamHandle,
                        BASSSync.BASS_SYNC_END,
                        0,
                        endTrackSyncProc,
                        IntPtr.Zero);

                    if (syncHandle == 0)
                        throw new ArgumentException("Error establishing End Sync on file stream.", "path");

                    CanPlay = true;
                }
                else
                {
                    Debug.WriteLine(string.Format("Failed to open file: {0},Error Code: {1}", path, Bass.BASS_ErrorGetCode()));
                }
            }
        }

        public void OpenUrl(string url)
        {
            openningStream = url;
            Stop();
            int handle = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_STREAM_RESTRATE, null, IntPtr.Zero);
            if (handle != 0)
            {
                if (openningStream == url)
                {
                    ActiveStreamHandle = handle;
                    ChannelLength = Bass.BASS_ChannelBytes2Seconds(ActiveStreamHandle, Bass.BASS_ChannelGetLength(ActiveStreamHandle));
                    var info = new BASS_CHANNELINFO();
                    Bass.BASS_ChannelGetInfo(ActiveStreamHandle, info);
                    sampleFrequency = info.freq;

                    int syncHandle = Bass.BASS_ChannelSetSync(ActiveStreamHandle,
                        BASSSync.BASS_SYNC_END,
                        0,
                        endTrackSyncProc,
                        IntPtr.Zero);

                    if (syncHandle == 0)
                        throw new ArgumentException("Error establishing End Sync on file stream.", "url");

                    CanPlay = true;
                }
                else
                {
                    if (!Un4seen.Bass.Bass.BASS_StreamFree(handle))
                    {
                        Debug.WriteLine("BASS_StreamFree() Failed：" + Un4seen.Bass.Bass.BASS_ErrorGetCode());
                    }
                }

            }
            else
            {
                Debug.WriteLine(string.Format("Failed to open URL: {0}, Error Code: {1}", url, Bass.BASS_ErrorGetCode()));
            }

        }

        #endregion

        #region Private Methods
        private void Initialize()
        {
            positionTimer.Interval = TimeSpan.FromMilliseconds(50);
            positionTimer.Tick += positionTimer_Tick;

            IsPlaying = false;

            IntPtr handle = IntPtr.Zero;
            //if (Application.Current.MainWindow != null)
            //{
            //    handle = new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle();
            //}

            var defaultDevice = FindDefaultDevice();
            var init = Bass.BASS_Init(defaultDevice, sampleFrequency, BASSInit.BASS_DEVICE_SPEAKERS, handle);
            if (init)
            {
                int pluginAAC = Bass.BASS_PluginLoad("bass_aac.dll");
#if DEBUG
                BASS_INFO info = new BASS_INFO();
                Bass.BASS_GetInfo(info);
                Debug.WriteLine(info.ToString());
                BASS_PLUGININFO aacInfo = Bass.BASS_PluginGetInfo(pluginAAC);
                foreach (BASS_PLUGINFORM f in aacInfo.formats)
                    Debug.WriteLine("Type={0}, Name={1}, Exts={2}", f.ctype, f.name, f.exts);
#endif
            }
            else
            {
                Debug.WriteLine("Bass Initialize error!");
            }

        }

        private void SetVolume()
        {
            if (ActiveStreamHandle != 0)
            {
                var value = IsMuted ? 0 : (float)Volume;
                Bass.BASS_ChannelSetAttribute(ActiveStreamHandle, BASSAttribute.BASS_ATTRIB_VOL, value);
            }
        }

        private void PlayCurrentStream()
        {
            // Play Stream
            if (ActiveStreamHandle != 0 && Bass.BASS_ChannelPlay(ActiveStreamHandle, false))
            {
                // Do nothing
            }
#if DEBUG
            else
            {
                Debug.WriteLine("Error={0}", Bass.BASS_ErrorGetCode());
            }
#endif
        }

        private void FreeCurrentStream()
        {
            if (ActiveStreamHandle != 0)
            {
                if (!Bass.BASS_StreamFree(ActiveStreamHandle))
                {
                    Debug.WriteLine("BASS_StreamFree失败：" + Un4seen.Bass.Bass.BASS_ErrorGetCode());
                }
                ActiveStreamHandle = 0;
            }
        }

        /// <summary>
        /// 查找设备的序号
        /// </summary>
        /// <param name="device">要查找的设备</param>
        /// <param name="returnDefault">当找不到设备时，是否返回默认设备的序号</param>
        /// <returns></returns>
        //private static int FindDevice(DeviceInfo? device, bool returnDefault = false)
        //{


        //    if (device.HasValue)
        //    {
        //        int deviceNO = -1;
        //        var devices = Bass.BASS_GetDeviceInfos().ToList();
        //        var filteredDevices = from d in devices where d.id != null && d.id == device.Value.ID select Array.IndexOf(devices, d);
        //        if (filteredDevices.Count() == 1)
        //        {
        //            deviceNO = filteredDevices.First();
        //        }
        //        if (deviceNO == -1)
        //        {
        //            filteredDevices = from d in devices where d.name == device.Value.Name select Array.IndexOf(devices, d);
        //            if (filteredDevices.Count() == 1)
        //            {
        //                deviceNO = filteredDevices.First();
        //            }
        //        }
        //        if (deviceNO == -1)
        //        {
        //            filteredDevices = from d in devices where d.driver == device.Value.Driver select Array.IndexOf(devices, d);
        //            if (filteredDevices.Count() == 1)
        //            {
        //                deviceNO = filteredDevices.First();
        //            }
        //        }
        //        if (deviceNO == -1 && returnDefault)
        //        {
        //            return FindDefaultDevice();
        //        }
        //        else if (deviceNO != -1)
        //        {
        //            return deviceNO;
        //        }
        //        else
        //        {
        //            throw new Exception("找不到此设备：" + device.Value.Name);
        //        }
        //    }
        //    else
        //    {
        //        return FindDefaultDevice();
        //    }
        //}

        /// <summary>
        /// 返回默认设备的序号
        /// </summary>
        /// <returns></returns>
        private static int FindDefaultDevice()
        {
            var devices = Bass.BASS_GetDeviceInfos();
            for (int i = 0; i < devices.Length; ++i)
            {
                if (devices[i].IsDefault) return i;
            }
            return 0;
            //throw new Exception("没有默认设备");
        }

        #endregion

        #region Event Handleres
        private void OnTrackEnded()
        {
            if (TrackEnded != null)
                TrackEnded(this, null);
        }

        private void positionTimer_Tick(object sender, EventArgs e)
        {
            if (ActiveStreamHandle == 0)
            {
                ChannelPosition = 0;
            }
            else
            {
                inChannelTimerUpdate = true;
                ChannelPosition = Bass.BASS_ChannelBytes2Seconds(ActiveStreamHandle, Bass.BASS_ChannelGetPosition(ActiveStreamHandle, 0));
                inChannelTimerUpdate = false;
            }
        }
        #endregion

        #region INotifyPopertyChanged
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
        public int GetFFTFrequencyIndex(int frequency)
        {
            return Utils.FFTFrequency2Index(frequency, fftDataSize, sampleFrequency);
        }

        public bool GetFFTData(float[] fftDataBuffer)
        {
            return (Bass.BASS_ChannelGetData(ActiveStreamHandle, fftDataBuffer, maxFFT)) > 0;
        }
        #endregion


        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //TODO:dispose BassEngine
                }
                disposed = true;
            }
        }
        #endregion
    }
}
