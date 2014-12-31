using SoundVisualizationLib.Universal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace DoubanFM.Universal
{
    public interface IPlayer : IDisposable, INotifyPropertyChanged, ISpectrumPlayer
    {
        bool CanPlay { get; set; }

        bool CanPause { get; set; }

        bool CanStop { get; set; }

        //Note:IsPlaying already defined in ISpectrumPlayer.
        //bool IsPlaying { get; set; }

        ICommand PlayPauseCommand { get; set; }

        event EventHandler TrackEnded;

        void OpenFile(string filePath);

        void OpenUrl(string url);

        void Stop();

        void Pause();

        void Play();
    }
}
