using DoubanFM.Audio;
using DoubanFM.Data;
using Microsoft.Practices.Prism.Commands;

namespace DoubanFM.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public MainWindowViewModel()
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
            }
        }


        //public BassEngine Player
        //{
        //    get
        //    {
        //        return BassEngine.Instance;
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
