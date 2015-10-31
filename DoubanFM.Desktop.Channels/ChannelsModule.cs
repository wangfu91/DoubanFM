using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace DoubanFM.Desktop.Channels
{
    public class ChannelsModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public ChannelsModule(IUnityContainer container, IRegionManager regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {
            //this._container.RegisterInstance<IChannelService>(new ChannelService());
			this._container.RegisterType<IChannelService, ChannelService>(new ContainerControlledLifetimeManager());
            this._regionManager.RegisterViewWithRegion(RegionNames.Channels,
                                                 () => this._container.Resolve<Views.ChannelListView>());
        }
    }
}
