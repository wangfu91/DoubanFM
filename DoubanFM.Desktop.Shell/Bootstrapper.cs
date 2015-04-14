using DoubanFM.Desktop.Resource.Controls;
using DoubanFM.Desktop.Shell.ViewModels;
using DoubanFM.Desktop.Shell.Views;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using System.Windows;
using Microsoft.Practices.Prism.PubSubEvents;

namespace DoubanFM.Desktop.Shell
{
    public class Bootstrapper : UnityBootstrapper
    {
        private readonly CallbackLogger _callbackLogger = new CallbackLogger();

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected override DependencyObject CreateShell()
        {
            //return ServiceLocator.Current.GetInstance<Views.ShellView>();

            //Use container to create an instance of the shell.
            var view = this.Container.TryResolve<ShellView>();
            return view;
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        /// <remarks>
        /// The base implemention ensures the shell is composed in the container.
        /// </remarks>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (WindowBase)this.Shell;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Create the <see cref="ILoggerFacade"/> Used by the bootstrapper.
        /// </summary>
        /// <returns></returns>
        protected override Microsoft.Practices.Prism.Logging.ILoggerFacade CreateLogger()
        {
            //return base.CreateLogger();

            return this._callbackLogger;
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. 
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.Container.RegisterType<IEventAggregator>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterInstance<CallbackLogger>(this._callbackLogger);

        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(DoubanFM.Desktop.NowPlaying.NowPlayingModule), InitializationMode.WhenAvailable);
            moduleCatalog.AddModule(typeof(DoubanFM.Desktop.Channels.ChannelsModule), InitializationMode.WhenAvailable);
            moduleCatalog.AddModule(typeof(DoubanFM.Desktop.Settings.SettingsModule), InitializationMode.WhenAvailable);
            moduleCatalog.AddModule(typeof(DoubanFM.Desktop.Account.AccountModule), InitializationMode.WhenAvailable);

        }
    }
}
