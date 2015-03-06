using DoubanFM.Desktop.ResourceLibrary;
using DoubanFM.Desktop.Shell.ViewModels;
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
using System.Windows.Shapes;

namespace DoubanFM.Desktop.Shell.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class ShellView : WindowBase
    {
        //The default parameter-less constructor is necessary to allow the view to work in design-time tools,
        //sunch as VS and Blend.
        public ShellView()
        {
            InitializeComponent();
        }

        public ShellView(ShellViewModel viewModel)
            : this()
        {
            this.DataContext = viewModel;
        }
    }
}
