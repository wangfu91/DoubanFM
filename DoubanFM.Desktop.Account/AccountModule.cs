using DoubanFM.Desktop.Account.Controllers;
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

namespace DoubanFM.Desktop.Account
{
	public class AccountModule : IModule
	{
		private readonly IUnityContainer _container;
		private readonly IRegionManager _regionManager;

		private AccountRegionController _regionController;

		public AccountModule(
			IUnityContainer container,
			IRegionManager regionManager)
		{
			this._container = container;
			this._regionManager = regionManager;
		}

		public void Initialize()
		{
			this._container.RegisterType<ILoginService, LoginService>(new ContainerControlledLifetimeManager());
			this._container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());

			this._regionManager.RegisterViewWithRegion(RegionNames.Account,
				() => this._container.Resolve<Views.AccountLoginView>());

			this._regionController = this._container.Resolve<AccountRegionController>();
		}
	}
}
