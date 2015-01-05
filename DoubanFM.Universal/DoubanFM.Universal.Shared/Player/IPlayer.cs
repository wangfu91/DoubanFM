using SoundVisualizationLib.Universal;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DoubanFM.Universal.Player
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

        Task OpenFile(string filePath);

        Task OpenUrl(string url);

        void Stop();

        void Pause();

        void Play();
    }
}
