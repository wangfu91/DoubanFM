using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Un4seen.Bass;
using WPFSoundVisualizationLib;

namespace DoubanFM.Desktop.Audio
{
	public sealed class BassEngine : IAudioEngine
	{
		#region Fileds
		private static BassEngine _instance;
		private bool _disposed;
		//private int _fileStreamhandle;
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
		private BASS_DEVICEINFO _deviceInfo;
		private BASSFlag openUrlConfig = (BASSFlag)Enum.Parse(typeof(BASSFlag), ConfigurationManager.AppSettings["Bass.OpenUrlConfig"]);
		private BASSFlag openFileConfig = (BASSFlag)Enum.Parse(typeof(BASSFlag), ConfigurationManager.AppSettings["Bass.OpenFileConfig"]);
		private readonly SYNCPROC endTrackSyncProc;
		private readonly DispatcherTimer positionTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
		private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
		private readonly int maxFFT = (int)(BASSData.BASS_DATA_AVAILABLE | BASSData.BASS_DATA_FFT2048);
		private readonly BASSInit _initFlags = (BASSInit)Enum.Parse(typeof(BASSInit), ConfigurationManager.AppSettings["Bass.InitFlags"]);
		private static readonly List<int> pluginHandles;
		private static readonly Dictionary<string, IntPtr> stringHandles = new Dictionary<string, IntPtr>();
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
					_instance = new BassEngine();
				return _instance;
			}
		}

		/// <summary>
		/// 显式初始化
		/// </summary>
		public static void ExplicitInitialize(BASS_DEVICEINFO deviceInfo = null)
		{
			if (_instance == null)
				_instance = new BassEngine(deviceInfo);
		}
		#endregion

		#region Notification Properties
		//public int FileStreamHandle
		//{
		//    get
		//    {
		//        return _fileStreamhandle;
		//    }
		//    private set
		//    {
		//        if (value != _fileStreamhandle)
		//        {
		//            _fileStreamhandle = value;
		//            NotifyPropertyChanged("FileStreamHandle");
		//        }
		//    }
		//}


		public int ActiveStreamHandle
		{
			get
			{
				return _activeStreamHandle;
			}
			private set
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
			private set
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
			private set
			{
				if (value != _canPlay)
				{
					_canPlay = value;
					NotifyPropertyChanged("CanPlay");
					//PlayCommand.RaiseCanExecuteChanged();
				}
			}
		}


		public bool CanPause
		{
			get
			{
				return _canPause;
			}
			private set
			{
				if (value != _canPause)
				{
					_canPause = value;
					NotifyPropertyChanged("CanPause");
					//PauseCommand.RaiseCanExecuteChanged();
				}
			}
		}



		public bool CanStop
		{
			get
			{
				return _canStop;
			}
			private set
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
			private set
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
			get { return _deviceInfo; }
			set
			{
				if (value != _deviceInfo)
				{
					_deviceInfo = value;
					ChangeDevice(_deviceInfo);
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


		static BassEngine()
		{
			//注册Bass.Net，不注册就会弹出一个启动画面
			BassNet.Registration("yk000123@sina.com", "2X34201017282922");
			//判断当前系统是32位系统还是64位系统，并加载对应版本的bass.dll
			string exeFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().GetModules()[0].FullyQualifiedName);
			string libraryPathSetting = Un4seen.Bass.Utils.Is64Bit ? "Bass.LibraryPathX64" : "Bass.LibraryPathX86";
			string bassDllBasePath = Path.Combine(exeFolder, ConfigurationManager.AppSettings[libraryPathSetting]);

			// now load all libs manually
			Un4seen.Bass.Bass.LoadMe(bassDllBasePath);
			var loadedPlugins =
				Un4seen.Bass.Bass.BASS_PluginLoadDirectory(
					string.Format(ConfigurationManager.AppSettings["Bass.PluginPathFormat"], bassDllBasePath));
			if (loadedPlugins != null)
			{
				foreach (var item in loadedPlugins)
				{
					Debug.WriteLine(string.Format("Plugin loaded: {0}", item.Value));
				}
				pluginHandles = loadedPlugins.Keys.ToList();
			}
			else
			{
				pluginHandles = new List<int>();
			}

			//BassMix.LoadMe(targetPath);
			//...
			//loadedPlugIns = Bass.BASS_PluginLoadDirectory(targetPath);
			//...

			SetConfigs(ConfigurationManager.AppSettings["Bass.SetConfigOnInitialization"]);

		}

		private BassEngine(BASS_DEVICEINFO deviceInfo = null)
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

			Initialize(deviceInfo);
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

		public async Task OpenFile(string fileName)
		{
			_openningStream = fileName;
			Stop();
			await Task.Run(() =>
			{
				if (ActiveStreamHandle != 0)
				{
					ChannelPosition = 0;
					Bass.BASS_StreamFree(ActiveStreamHandle);
				}

				if (File.Exists(fileName))
				{
					ActiveStreamHandle = Bass.BASS_StreamCreateFile(fileName, 0, 0, openFileConfig);
					if (ActiveStreamHandle != 0)
					{

						ChannelLength = Bass.BASS_ChannelBytes2Seconds(ActiveStreamHandle, Bass.BASS_ChannelGetLength(ActiveStreamHandle, 0));

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
						Debug.WriteLine(string.Format("Failed to open file: {0},Error Code: {1}", fileName, Bass.BASS_ErrorGetCode()));
					}
				}
			});
		}

		public async Task OpenUrl(string url)
		{
			_openningStream = url;
			Stop();
			await Task.Run(() =>
			{
				int handle = Bass.BASS_StreamCreateURL(url, 0, openUrlConfig, null, IntPtr.Zero);
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
			});

		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Set a config with string value type.
		/// </summary>
		/// <param name="config">config name</param>
		/// <param name="value">string value</param>
		/// <returns>success or not</returns>
		private static bool SetConfig(BASSConfig config, string value)
		{
			var configName = config.ToString();
			if (stringHandles.ContainsKey(configName) && stringHandles[configName] != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(stringHandles[configName]);
				stringHandles.Remove(configName);
			}

			var handle = value == null ? IntPtr.Zero : Marshal.StringToHGlobalAnsi(value);
			if (Un4seen.Bass.Bass.BASS_SetConfigPtr(config, handle))
			{
				if (handle != IntPtr.Zero)
				{
					stringHandles[configName] = handle;
				}
				return true;
			}
			if (handle != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(handle);
			}
			return false;
		}

		/// <summary>
		/// Set configs presented as a string.
		/// </summary>
		/// <param name="configs">The configs.</param>
		/// <exception cref="System.IO.InvalidDataException">
		/// </exception>
		/// <exception cref="System.Exception">
		/// </exception>
		private static void SetConfigs(string configs)
		{
			foreach (var config in configs.Split('|'))
			{
				if (config == string.Empty) continue;

				var spaceIndex = config.IndexOf(' ');
				if (spaceIndex == -1)
				{
					throw new InvalidDataException(string.Format("Config 'Bass.SetConfigOnInitialization' is invalid. Invalid config string: {0}", config));
				}
				var configNameString = config.Substring(0, spaceIndex);
				BASSConfig configName;
				if (!BASSConfig.TryParse(configNameString, out configName)
					|| !Enum.IsDefined(typeof(BASSConfig), configName))
				{
					throw new InvalidDataException(string.Format("Config 'Bass.SetConfigOnInitialization' is invalid. Invalid config name: {0}", configNameString));
				}

				var configValueString = config.Substring(spaceIndex + 1);
				int configValueInt;
				if (int.TryParse(configValueString, out configValueInt))
				{
					if (!Bass.BASS_SetConfig(configName, configValueInt))
					{
						throw new Exception(string.Format("Set config {0} with value {1} failed. Error code {2}",
							configName, configValueInt, Bass.BASS_ErrorGetCode()));
					}
					continue;
				}
				bool configValueBool;
				if (bool.TryParse(configValueString, out configValueBool))
				{
					if (!Bass.BASS_SetConfig(configName, configValueBool))
					{
						throw new Exception(string.Format("Set config {0} with value {1} failed. Error code {2}",
							configName, configValueBool, Bass.BASS_ErrorGetCode()));
					}
					continue;
				}
				if (!SetConfig(configName, configValueString))
				{
					throw new Exception(string.Format("Set config {0} with value {1} failed. Error code {2}",
							configName, configValueString, Bass.BASS_ErrorGetCode()));
				}
			}

		}

		private void Initialize(BASS_DEVICEINFO device = null)
		{
			positionTimer.Interval = TimeSpan.FromMilliseconds(50);
			positionTimer.Tick += positionTimer_Tick;

			IsPlaying = false;

			IntPtr handle = IntPtr.Zero;
			if (Application.Current.MainWindow != null)
			{
				handle = new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle();
			}
			//The device to use... -1 = default device, 0 = no sound, 1 = first real output device. 
			var deviceNO = FindDevice(device, true);

			var init = Bass.BASS_Init(deviceNO, sampleFrequency, _initFlags, handle);
			if (init)
			{
				var error = Bass.BASS_ErrorGetCode();
				int count = Bass.BASS_GetDeviceCount();
				for (deviceNO = -1; deviceNO < count; ++deviceNO)
				{
					if (deviceNO != 0 && Un4seen.Bass.Bass.BASS_Init(deviceNO, sampleFrequency, _initFlags, handle))
					{
						break;
					}
				}
				if (deviceNO == count)
				{
					throw new BassInitializationFailureException(error);
				}

			}

			if (device == null && deviceNO == FindDefaultDevice())
			{
				Device = null;
			}
			else
			{
				Device = Bass.BASS_GetDeviceInfo(Bass.BASS_GetDevice());
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
			if (device != null)
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
			else
			{
				return FindDefaultDevice();
			}
		}


		/// <summary>
		/// 返回默认设备的序号
		/// </summary>
		/// <returns></returns>
		private static int FindDefaultDevice()
		{
			var devices = Un4seen.Bass.Bass.BASS_GetDeviceInfos();
			for (int i = 0; i < devices.Length; ++i)
			{
				if (devices[i].IsDefault) return i;
			}
			throw new Exception("没有默认设备");
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
