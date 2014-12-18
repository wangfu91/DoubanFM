using DoubanFM.Audio;
using DoubanFM.Common;
using DoubanFM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.LocalPlayer.ViewModels
{
    public class PlayerUIViewModel:ViewModelBase
    {
        private IAudioEngine playEngine;

        public IAudioEngine Player
        {
            get { return playEngine; }
        }

        public PlayerUIViewModel(IAudioEngine playEngine)
        {
            this.playEngine = playEngine;

            this.playEngine.OpenFile(@"C:\You Raise Me Up.mp3");

            this.playEngine.Play();
        }
    }
}
