using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DoubanFM.Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : VisualStateAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DrawerLayout.InitializeDrawerLayout();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if(DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.CloseDrawer();
                e.Handled = true;
            }
            else
            {
                Application.Current.Exit();
            }
        }

        private void DrawerIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(this.DrawerLayout.IsDrawerOpen)
            {
                DrawerLayout.CloseDrawer();
            }
            else
            {
                DrawerLayout.OpenDrawer();
            }
        }
    }
}
