using DoubanFM.Desktop.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;

namespace DoubanFM.Desktop.Search
{
    public class SearchModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public SearchModule(IUnityContainer container, IRegionManager regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._container.RegisterType<IEventAggregator>(new ContainerControlledLifetimeManager());

            this._regionManager.RegisterViewWithRegion(RegionNames.Search,
                                                 () => this._container.Resolve<Views.SearchView>());
        }
    }
}
