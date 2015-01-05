using DoubanFM.Desktop.Audio;
using DoubanFM.Desktop.Infrastructure;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DoubanFM.Desktop.Core
{
    public class ModuleInit : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;


        public ModuleInit(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            this.container.RegisterInstance<IAudioEngine>(BassEngine.Instance);

            regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                 () => this.container.Resolve<Views.PlayerUI>());

        }
    }
}
