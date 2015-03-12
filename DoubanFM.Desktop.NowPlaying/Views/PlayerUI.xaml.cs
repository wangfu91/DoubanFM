using DoubanFM.Desktop.NowPlaying.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoubanFM.Desktop.NowPlaying.Views
{
    /// <summary>
    /// Interaction logic for PlayerUI.xaml
    /// </summary>
    public partial class PlayerUI : UserControl
    {
        //The default parameter-less constructor is necessary to allow the view to work in design-time tools,
        //sunch as VS and Blend.
        public PlayerUI()
        {
            InitializeComponent();
        }

        public PlayerUI(PlayerUIViewModel viewModel)
            : this()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.spectrumAnalyzer.RegisterSoundPlayer(viewModel.Player);
        }
    }
}
