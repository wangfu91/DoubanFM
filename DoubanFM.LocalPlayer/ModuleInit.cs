using DoubanFM.Audio;
using DoubanFM.Common;
using DoubanFM.LocalPlayer.ViewModels;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DoubanFM.LocalPlayer
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
            this.container.RegisterInstance<IPlayEngine>(BassEngine.Instance);

            regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                 () => this.container.Resolve<Views.PlayerUI>());
            regionManager.RegisterViewWithRegion(RegionNames.PlayControlRegin,
                                                 () => this.container.Resolve<Views.PlayControl>());
        }
    }
}
