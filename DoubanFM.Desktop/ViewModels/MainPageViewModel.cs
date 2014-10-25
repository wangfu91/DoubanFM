using DoubanFM.Audio;
using Microsoft.Practices.Prism.Commands;

namespace DoubanFM.Desktop.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private NAudioEngine player;
        public MainPageViewModel()
        {
            this.LoadedCommand = new DelegateCommand(async () => await Player.Initialize(@"C:\Users\WangFu\Desktop\You Raise Me Up.mp3"));

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

        private int myVar;

        public int MyProperty
        {
            get
            {
                return myVar;
            }
            set
            {
                if (value != myVar)
                {
                    myVar = value;
                    OnPropertyChanged(() => this.MyProperty);
                }
            }
        }




        public DelegateCommand LoadedCommand { get; set; }





    }
}
