using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFSoundVisualizationLib;

namespace DoubanFM.Audio
{
    public interface IPlayerEngine :IDisposable,INotifyPropertyChanged,ISpectrumPlayer
    {
        bool CanPlay { get; set; }

        bool CanPause { get; set; }

        bool CanStop { get; set; }

        //IsPlaying already defined in ISpectrumPlayer.
        //bool IsPlaying { get; set; }

        bool IsLiked { get; set; }

        ICommand PlayPauseCommand { get; set; }

        ICommand PlayNextCommand { get; set; }

        void Stop();

        void Pause();

        void Play();
    }
}
