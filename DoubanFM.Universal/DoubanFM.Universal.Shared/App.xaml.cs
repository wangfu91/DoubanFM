using DoubanFM.Universal.APIs.Models;
using DoubanFM.Universal.APIs.Services;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;


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