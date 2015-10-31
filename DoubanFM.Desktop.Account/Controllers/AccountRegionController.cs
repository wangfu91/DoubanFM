using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using System;

namespace DoubanFM.Desktop.Account.Controllers
{
    /// <summary>
    /// Used to programmatically switch views in the region when user logged in or logged out.
    /// </summary>
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

            this.eventAggregator.GetEvent<UserStateChangedEvent>().Subscribe(HandleUserStateChange);
        }

        private void HandleUserStateChange(LoginResult result)
        {
            if (result != null)
                UserLoggedIn(result);
            else
                UserLoggedOut();
        }


        private void UserLoggedIn(LoginResult result)
        {
            this.container.RegisterInstance<LoginResult>(result);

            var accountRegion = this.regionManager.Regions[RegionNames.Account];
            if (accountRegion == null) return;

            /*
            *  var loginView = accountRegion.GetView("AccountLoginView"); //this line failed to get the expected view, always return null, TODO: need investrigate.
            *  if(loginView!=null) //loginView  is null, 
            *  {
            *      accountRegion.Remove(loginView);
            *  }
			*/

            //Remove all the view hosted in account region so far.
            foreach (var item in accountRegion.Views)
            {
                accountRegion.Remove(item);
            }

            //Add userInfo view to the region, this will automatically active the view too.
            var userInfoView = this.container.Resolve<Views.UserInfoView>();
            accountRegion.Add(userInfoView);
        }

        private void UserLoggedOut()
        {
            var accountRegion = this.regionManager.Regions[RegionNames.Account];
            if (accountRegion == null) return;

            //Remove all the view hosted in account region so far.
            foreach (var item in accountRegion.Views)
            {
                accountRegion.Remove(item);
            }

            //Add login view view to the region, this will automatically active the view too.
            var loginView = this.container.Resolve<Views.AccountLoginView>();
            accountRegion.Add(loginView);

        }
    }
}
