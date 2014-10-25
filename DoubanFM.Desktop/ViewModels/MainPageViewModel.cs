using DoubanFM.Audio;
using DoubanFM.Data;
using Microsoft.Practices.Prism.Commands;

namespace DoubanFM.Desktop.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private NAudioEngine player;
        public MainPageViewModel()
        {
            this.LoadedCommand = new DelegateCommand(async () =>
                {
                    await Player.PlayNext();
                });
        }

        public NAudioEngine Player
        {
            get
            {
                if (player == null)
                    player = NAudioEngine.Instance;
                return player;
            }
        }


        private Song currentSong;

        public Song CurrentSong
        {
            get
            {
                return currentSong;
            }
            set
            {
                if (value != currentSong)
                {
                    currentSong = value;
                    OnPropertyChanged(() => this.CurrentSong);
                }
            }
        }



        public DelegateCommand LoadedCommand { get; set; }

    }
}
