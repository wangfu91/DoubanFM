using DoubanFM.Desktop.Core.ViewModels;
using System.Windows.Controls;

namespace DoubanFM.Desktop.Core.Views
{
    /// <summary>
    /// Interaction logic for PlayerUI.xaml
    /// </summary>
    public partial class PlayerUI : UserControl
    {
        public PlayerUI(PlayerUIViewModel playUIViewModel)
        {
            InitializeComponent();
            this.DataContext = playUIViewModel;
            this.spectrumAnalyzer.RegisterSoundPlayer(playUIViewModel.Player);
        }
    }
}
