using DoubanFM.Audio;
using DoubanFM.Data;
using Microsoft.Practices.Prism.Commands;

namespace DoubanFM.Desktop.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        //private NAudioEngine player;
        //private BassEngine player=BassEngine.Instance;

        public MainPageViewModel()
        {
            this.LoadedCommand = new DelegateCommand(() =>
                {

                    Player.OpenFile(@"C:\Users\WangFu\Desktop\You Raise Me Up.mp3");

                    Player.Play();
                });
        }

        public NAudioEngine Player
        {
            get
            {
                return NAudioEngine.Instance;
                //if(player==null)
                //{
                //    player = NAudioEngine.Instance;
                //}
                //return player;
            }
        }


        //public BassEngine Player
        //{
        //    get
        //    {
        //        if(player==null)
        //        {
        //            player = BassEngine.Instance;
        //        }
        //        return player;
        //    }
        //}


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
