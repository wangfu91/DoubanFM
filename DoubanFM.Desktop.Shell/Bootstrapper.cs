using DoubanFM.Desktop.ResourceLibrary;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using System.Windows;

namespace DoubanFM.Desktop.Shell
{
    public class Bootstrapper : UnityBootstrapper
    {
        private readonly CallbackLogger callbackLogger = new CallbackLogger();

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<Shell>();
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
            return base.CreateLogger();

            //return this.callbackLogger;
        }

        /// <summary>
        /// Configures the <see cref="IUnityContainer"/>. 
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();


            //this.Container.RegisterInstance<CallbackLogger>(this.callbackLogger);

        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            var moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(DoubanFM.Desktop.Core.ModuleInit), InitializationMode.WhenAvailable);

        }
    }
}
