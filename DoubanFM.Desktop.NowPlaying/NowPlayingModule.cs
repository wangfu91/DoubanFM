using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Audio;
using DoubanFM.Desktop.Infrastructure;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Microsoft.Practices.Unity;


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
            this._container.RegisterType<IEventAggregator>(new ContainerControlledLifetimeManager());
            this._container.RegisterInstance<IAudioEngine>(BassEngine.Instance);
            this._container.RegisterType<ISongService, SongService>(new ContainerControlledLifetimeManager());
            this._container.RegisterType<ILyricsService, LyricsService>(new ContainerControlledLifetimeManager());

            this._regionManager.RegisterViewWithRegion(RegionNames.NowPlaying,
                                                 () => this._container.Resolve<Views.NowPlayingView>());
        }
    }
}
