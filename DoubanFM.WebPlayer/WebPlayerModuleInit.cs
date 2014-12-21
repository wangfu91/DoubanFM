using DoubanFM.Audio;
using DoubanFM.Common;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace DoubanFM.WebPlayer
{
    public class WebPlayerModuleInit : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;


        public WebPlayerModuleInit(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            this.container.RegisterInstance<IAudioEngine>(BassEngine.Instance);

            regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                 () => this.container.Resolve<Views.PlayerUI>());
            //regionManager.RegisterViewWithRegion(RegionNames.PlayControlRegin,
            //                                     () => this.container.Resolve<Views.PlayControl>());
            //regionManager.RegisterViewWithRegion(RegionNames.PlayListRegion,
            //                         () => this.container.Resolve<Views.ChannelList>());

        }
    }
}
