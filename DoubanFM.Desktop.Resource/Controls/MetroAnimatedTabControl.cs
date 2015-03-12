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

namespace DoubanFM.Desktop.Resource.Controls
{

    /// <summary>
    /// A MetroTabControl (Pivot) that uses a TransitioningContentControl to animate the contents of a TabItem/MetroTabItem.
    /// </summary>
    public class MetroAnimatedTabControl : BaseMetroTabControl
    {

        /// <summary>
        /// Initializes a new instance of the MahApps.Metro.Controls.MetroAnimatedTabControl class.
        /// </summary>
        public MetroAnimatedTabControl()
        {
            DefaultStyleKey = typeof(MetroAnimatedTabControl);
        }
    }
}
