using System;
using System.ComponentModel;
using System.Windows.Input;
using WPFSoundVisualizationLib;

namespace DoubanFM.Desktop.Audio
{
	public interface IAudioEngine : IDisposable, INotifyPropertyChanged, ISpectrumPlayer
	{
		bool CanPlay { get;}

		bool CanPause { get; }

		bool CanStop { get;  }

        bool IsMuted { get; set; }

        double Volume { get; set; }

		double ChannelPosition { get; set; }

		double ChannelLength { get;}

		//Note:IsPlaying already defined in ISpectrumPlayer.
		//bool IsPlaying { get; set; }

		ICommand PlayCommand { get; set; }

		ICommand PauseCommand { get; set; }

		event EventHandler TrackEnded;

		void OpenFile(string filePath);

		void OpenUrl(string url);

		void Stop();

		void Pause();

		void Play();
	}
}
