using DoubanFM.Audio;
using DoubanFM.WebPlayer.ViewModels;
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

namespace DoubanFM.WebPlayer.Views
{
    /// <summary>
    /// Interaction logic for PlayControl.xaml
    /// </summary>
    public partial class PlayControl : UserControl
    {
        public PlayControl(PlayerUIViewModel playUIViewModel)
        {
            InitializeComponent();

            this.DataContext = playUIViewModel;
        }
    }
}
