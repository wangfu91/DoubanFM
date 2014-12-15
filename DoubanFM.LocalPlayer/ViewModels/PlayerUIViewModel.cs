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
        private IPlayEngine playEngine;

        public IPlayEngine Player
        {
            get { return playEngine; }
            set
            {
                if (value != playEngine)
                {
                    playEngine = value;
                    OnPropertyChanged(() => this.Player);
                }
            }
        }

        public PlayerUIViewModel(IPlayEngine playEngine)
        {
            this.playEngine = playEngine;

            this.playEngine.OpenFile(@"C:\Users\Frank\Music\You Raise Me Up.mp3");

            this.playEngine.Play();
        }
    }
}
