using DoubanFM.LocalPlayer.ViewModels;
using Microsoft.Practices.Prism.Mvvm;
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

namespace DoubanFM.LocalPlayer.Views
{
    /// <summary>
    /// Interaction logic for PlayerUI.xaml
    /// </summary>
    public partial class PlayerUI : UserControl,IView
    {
        public PlayerUI(PlayerUIViewModel playUIViewModel)
        {
            InitializeComponent();

            this.DataContext = playUIViewModel;
            this.spectrumAnalyzer.RegisterSoundPlayer(playUIViewModel.Player);

        }
    }
}
