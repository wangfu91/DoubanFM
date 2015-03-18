using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
