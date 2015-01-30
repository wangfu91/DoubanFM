using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Un4seen.Bass;
using WPFSoundVisualizationLib;

namespace DoubanFM.Desktop.Audio
{
	public class BassEngine : IAudioEngine
	{
		#region Fileds
		private static BassEngine _instance;
		private bool _disposed;
		private int _fileStreamhandle;
		private int _activeStreamHandle;
		private double _channelLength;
		private bool _canPlay;
		private bool _canPause;
		private bool _canStop;
		private bool _isPlaying;
		private bool _isMuted;
		private double _volume = 1.0;//default volume is 100%
		private double currentChannelPosition;
		private bool inChannelSet;
		private bool inChannelTimerUpdate;
		private int sampleFrequency = 44100; //44.1KHZ
											 /// <summary>
											 /// 保存正在打开的文件的地址，当短时间内多次打开网络文件时，这个字段保存最后一次打开的文件，可以使其他打开文件的操作失效
											 /// </summary>
		private string _openningStream = null;
		private BASS_DEVICEINFO _device;

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
				if (_instance == null)
				{
					_instance = new BassEngine();
				}
				return _instance;
			}
		}
		#endregion

		#region Notification Properties
		public int FileStreamHandle
		{
			get
			{
				return _fileStreamhandle;
			}
			set
			{
				if (value != _fileStreamhandle)
				{
					_fileStreamhandle = value;
					NotifyPropertyChanged("FileStreamHandle");
				}
			}
		}


		public int ActiveStreamHandle
		{
			get
			{
				return _activeStreamHandle;
			}
			set
			{
				if (value != _activeStreamHandle)
				{
					_activeStreamHandle = value;
					if (_activeStreamHandle != 0)
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
				return _channelLength;
			}
			set
			{
				if (value != _channelLength)
				{
					_channelLength = value;
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
					var position = Math.Max(0, Math.Min(value, _channelLength));
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
				return _canPlay;
			}
			set
			{
				if (value != _canPlay)
				{
					_canPlay = value;
					NotifyPropertyChanged("CanPlay");
					//PlayOrPauseCommand.RaiseCanExecuteChanged();
				}
			}
		}


		public bool CanPause
		{
			get
			{
				return _canPause;
			}
			set
			{
				if (value != _canPause)
				{
					_canPause = value;
					NotifyPropertyChanged("CanPause");
				}
			}
		}



		public bool CanStop
		{
			get
			{
				return _canStop;
			}
			set
			{
				if (value != _canStop)
				{
					_canStop = value;
					NotifyPropertyChanged("CanStop");
					//StopCommand.RaiseCanExecuteChanged();
				}
			}
		}



		public bool IsPlaying
		{
			get
			{
				return _isPlaying;
			}
			set
			{
				if (value != _isPlaying)
				{
					_isPlaying = value;
					positionTimer.IsEnabled = value;
					NotifyPropertyChanged("IsPlaying");
				}
			}
		}


		public bool IsMuted
		{
			get { return _isMuted; }
			set
			{
				if (value != _isMuted)
				{
					_isMuted = value;
					SetVolume();
					NotifyPropertyChanged("IsMute");
				}
			}
		}

		public double Volume
		{
			get { return _volume; }
			set
			{
				value = Math.Max(0, Math.Min(1, value));
				if (value != _volume)
				{
					_volume = value;
					SetVolume();
					NotifyPropertyChanged("Volume");
				}
			}
		}

		public BASS_DEVICEINFO Device
		{
			get { return _device; }
			set
			{
				if (value != _device)
				{
					_device = value;
					NotifyPropertyChanged("Device");
				}
			}
		}

		#endregion

		#region ICommands
		public ICommand PlayCommand { get; set; }

		public ICommand PauseCommand { get; set; }

		public ICommand StopCommand { get; set; }

		public ICommand PlayNextCommand { get; set; }

		public ICommand DeleteCommand { get; set; }

		#endregion

		#region  Constructor

		private BassEngine()
		{
			this.PlayCommand = new DelegateCommand(() =>
			{
				if (!IsPlaying)
					Play();
			}, () => this.CanPlay);

			this.PauseCommand = new DelegateCommand(() =>
			  {
				  if (IsPlaying)
					  Pause();
			  }, () => this.CanPause);

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
			if (CanPlay)
			{
				PlayCurrentStream();
				IsPlaying = true;
				CanPause = true;
				CanPlay = false;
				CanStop = true;
			}
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
			_openningStream = url;
			Stop();
			int handle = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_STREAM_RESTRATE, null, IntPtr.Zero);
			if (handle != 0)
			{
				if (_openningStream == url)
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
			//The device to use... -1 = default device, 0 = no sound, 1 = first real output device. 
			var defaultDevice = -1;
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

		#region  Utility Methods

		public static List<BASS_DEVICEINFO> GetAvailableDevices()
		{
			var results = new List<BASS_DEVICEINFO>();
			var devices = Bass.BASS_GetDeviceInfos().ToList();
			foreach (var device in devices)
			{
				var comparison = StringComparison.InvariantCultureIgnoreCase;
				if (device.IsEnabled && !string.Equals(device.name, "No sound", comparison))
				{
					results.Add(device);
				}
			}
			return results;
		}


		public void ChangeDevice(BASS_DEVICEINFO device)
		{
			var deviceNO = FindDevice(device);
			var oldDeviceNO = Bass.BASS_GetDevice();
			if (oldDeviceNO != deviceNO)
			{
				if (!Bass.BASS_GetDeviceInfo(deviceNO).IsInitialized)
				{
					var handle = IntPtr.Zero;
					if (!Bass.BASS_Init(-1, sampleFrequency, BASSInit.BASS_DEVICE_SPEAKERS, handle))
					{
						Debug.WriteLine("Bass Initialize error!");
						throw new Exception(Bass.BASS_ErrorGetCode().ToString());
					}
				}
				if (_activeStreamHandle != 0)
				{
					if (!Bass.BASS_ChannelSetDevice(_activeStreamHandle, deviceNO))
					{
						throw new Exception(Un4seen.Bass.Bass.BASS_ErrorGetCode().ToString());
					}
				}
				if (!Un4seen.Bass.Bass.BASS_SetDevice(oldDeviceNO))
				{
					throw new Exception(Un4seen.Bass.Bass.BASS_ErrorGetCode().ToString());
				}
				if (!Un4seen.Bass.Bass.BASS_Free())
				{
					throw new Exception(Un4seen.Bass.Bass.BASS_ErrorGetCode().ToString());
				}
				if (!Un4seen.Bass.Bass.BASS_SetDevice(deviceNO))
				{
					throw new Exception(Un4seen.Bass.Bass.BASS_ErrorGetCode().ToString());
				}
			}
			Device = device;
		}


		/// <summary>
		/// 查找设备的序号
		/// </summary>
		/// <param name="device">要查找的设备</param>
		/// <param name="returnDefault">当找不到设备时，是否返回默认设备的序号</param>
		/// <returns></returns>
		private static int FindDevice(BASS_DEVICEINFO device, bool returnDefault = false)
		{
			int deviceNO = -1;
			var devices = Bass.BASS_GetDeviceInfos();

			var filteredDevices =
			   from d in devices
			   where d.name == device.name
			   select Array.IndexOf(devices, d);
			if (deviceNO == -1)
			{
				if (filteredDevices.Count() == 1)
				{
					deviceNO = filteredDevices.First();
				}
			}
			if (deviceNO == -1)
			{
				filteredDevices =
					from d in devices
					where d.driver == device.driver
					select Array.IndexOf(devices, d);
				if (filteredDevices.Count() == 1)
				{
					deviceNO = filteredDevices.First();
				}
			}
			if (deviceNO == -1 && returnDefault)
			{
				return deviceNO;
			}
			else if (deviceNO != -1)
			{
				return deviceNO;
			}
			else
			{
				throw new Exception("找不到此设备：" + device.name);
			}
		}


		/// <summary>
		/// 返回默认设备的序号
		/// </summary>
		/// <returns></returns>
		private static BASS_DEVICEINFO GetDefaultDevice()
		{
			var devices = Bass.BASS_GetDeviceInfos().ToList();
			if (devices.Where(d => d.IsDefault).Count() > 0)
			{
				var device = devices.First();
				return device;
			}
			else
			{
				throw new Exception("没有默认设备");
			}
		}

		#endregion

		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					//TODO:dispose BassEngine
				}
				_disposed = true;
			}
		}
		#endregion
	}
}
