using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace DoubanFM.LocalPlayer
{
    public class LocalPlayerModule : IModule
    {
        private readonly IRegionManager regionManager;


        public LocalPlayerModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }


        public void Initialize()
        {
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.PlayList));
        }
    }
}
