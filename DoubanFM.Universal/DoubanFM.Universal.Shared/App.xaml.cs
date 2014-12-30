using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using DoubanFM.Universal.Common;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.PubSubEvents;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Prism.StoreApps;
using Windows.ApplicationModel.Resources;
using DoubanFM.Universal.APIs.Models;
using DoubanFM.Universal.APIs.Services;


// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace DoubanFM.Universal
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : MvvmAppBase
    {
        //create the singleton container that will be used for type resolution in the app.
        private readonly IUnityContainer container = new UnityContainer();


        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Initializes the singleton instance of the <see cref="App"/> class. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (args != null && !string.IsNullOrEmpty(args.Arguments))
            {
                //The app was launched from a Secondary Tile
                //Navigate to the Item's page
                NavigationService.Navigate("Item", args.Arguments);
            }
            else
            {
                //Navigation to the initial page
                NavigationService.Navigate("Main", null);
            }

            Window.Current.Activate();
            return Task.FromResult<object>(null);

        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            //setup the list for known type for the SuspensionManager
            SessionStateService.RegisterKnownType(typeof(Channel));
            SessionStateService.RegisterKnownType(typeof(ChannelList));
            SessionStateService.RegisterKnownType(typeof(Song));
            SessionStateService.RegisterKnownType(typeof(Queue<Song>));
            SessionStateService.RegisterKnownType(typeof(User));


            base.OnRegisterKnownTypesForSerialization();
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            this.EventAggregator = new EventAggregator();

            container.RegisterInstance<INavigationService>(NavigationService);
            container.RegisterInstance<ISessionStateService>(SessionStateService);
            container.RegisterInstance<IEventAggregator>(EventAggregator);
            container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            //container.RegisterInstance(typeof(ChannelService), new ChannelService());
            //container.RegisterInstance(typeof(UserService), new UserService());

            container.RegisterType<ILoginService, LoginService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserService,UserService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IChannelService, ChannelService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISongService, SongService>(new ContainerControlledLifetimeManager());


            return base.OnInitializeAsync(args);
        }

        protected override object Resolve(Type type)
        {
            //use the container to resolve types,
            //so their dependencies get injected
            return base.Resolve(type);
        }

    }
}