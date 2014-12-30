using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundVisualizationLib.Universal
{
    public interface ISoundPlayer:INotifyPropertyChanged
    {
        bool IsPlaying { get; }
    }
}
