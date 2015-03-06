using DoubanFM.Desktop.Audio;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.NowPlaying.ViewModels;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.NowPlaying
{
    public class NowPlayingModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public NowPlayingModule(IUnityContainer container, IRegionManager regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._container.RegisterInstance<IAudioEngine>(BassEngine.Instance);

            this._regionManager.RegisterViewWithRegion(RegionNames.NowPlaying,
                                                 () => this._container.Resolve<Views.PlayerUI>());
        }
    }
}
