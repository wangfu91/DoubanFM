using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.Account.Controllers
{
    public class AccountRegionController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public AccountRegionController(
            IUnityContainer container,
            IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            this.eventAggregator.GetEvent<UserLoggedInEvent>().Subscribe(UserLoggedIn);
        }

        private void UserLoggedIn(LoginResult result)
        {
            this.container.RegisterInstance<LoginResult>(result);

            var accountRegion = this.regionManager.Regions[RegionNames.Account];
            if (accountRegion == null) return;
            var loginView = accountRegion.GetView("AccountLoginView");
            if(loginView!=null)
            {
                accountRegion.Remove(loginView);
            }

			foreach (var item in accountRegion.Views)
			{
				accountRegion.Remove(item);
			}

            var userInfoView = this.container.Resolve<Views.UserInfoView>();
            accountRegion.Add(userInfoView);
        }
    }
}
