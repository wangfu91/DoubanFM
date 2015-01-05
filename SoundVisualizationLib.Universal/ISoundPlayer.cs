using System.ComponentModel;

namespace SoundVisualizationLib.Universal
{
    public interface ISoundPlayer:INotifyPropertyChanged
    {
        bool IsPlaying { get; }
    }
}
