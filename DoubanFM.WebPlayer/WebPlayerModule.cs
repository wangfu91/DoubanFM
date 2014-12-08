using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace DoubanFM.WebPlayer
{
    public class WebPlayerModule : IModule
    {
        private readonly IRegionManager regionManager;

        public WebPlayerModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.ChannelList));
        }
    }
}
