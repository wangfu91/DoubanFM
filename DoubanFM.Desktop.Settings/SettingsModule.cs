using DoubanFM.Desktop.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace DoubanFM.Desktop.Settings
{
    public class SettingsModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public SettingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._regionManager.RegisterViewWithRegion(RegionNames.Settings,
                                                 () => this._container.Resolve<Views.SettingsView>());
        }
    }
}
