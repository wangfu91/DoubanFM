using System.Windows;

namespace DoubanFM.Desktop.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //The bootstrapper will create the shell instance,
            //so the App.xaml does not have a StartupUri.
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
