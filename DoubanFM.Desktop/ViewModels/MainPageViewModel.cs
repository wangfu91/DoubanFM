using DoubanFM.Audio;
using DoubanFM.Data;
using Microsoft.Practices.Prism.Commands;

namespace DoubanFM.Desktop.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        //private NAudioEngine player;
        private BassEngine player;

        public MainPageViewModel()
        {
            this.LoadedCommand = new DelegateCommand(/*async */ () =>
                {
                    //await Player.PlayNext();
                    Player.OpenFile(@"C:\Users\Frank\Desktop\Need You Now.mp3");
                    Player.Play();
                });
        }

        //public NAudioEngine Player
        //{
        //    get
        //    {
        //        return player ?? NAudioEngine.Instance;
        //    }
        //}


        public BassEngine Player
        {
            get
            {
                return player ?? BassEngine.Instance;                 
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
