using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
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
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ICredentialManageService _credentialStorageService;

        public AccountRegionController(
            IUnityContainer container,
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            ICredentialManageService credentialStorageService)
        {
            this._container = container;
            this._regionManager = regionManager;
            this._eventAggregator = eventAggregator;
            this._credentialStorageService = credentialStorageService;

            this._eventAggregator.GetEvent<UserStateChangedEvent>().Subscribe(HandleUserStateChange);
            TryLoadCredentialAsync();
        }

        public async void TryLoadCredentialAsync()
        {
            var loginResult = await _credentialStorageService.LoadCredentialAsync();
            HandleUserStateChange(loginResult);
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
            this._container.RegisterInstance(result);

            var accountRegion = this._regionManager.Regions[RegionNames.Account];
            if (accountRegion == null) return;

            //Remove all the view hosted in account region so far.
            foreach (var item in accountRegion.Views)
            {
                accountRegion.Remove(item);
            }

            //Add userInfo view to the region, this will automatically active the view too.
            var userInfoView = this._container.Resolve<Views.UserInfoView>();
            accountRegion.Add(userInfoView);
        }

        private void UserLoggedOut()
        {
            var accountRegion = this._regionManager.Regions[RegionNames.Account];
            if (accountRegion == null) return;

            //Remove all the view hosted in account region so far.
            foreach (var item in accountRegion.Views)
            {
                accountRegion.Remove(item);
            }

            //Add login view view to the region, this will automatically active the view too.
            var loginView = this._container.Resolve<Views.AccountLoginView>();
            accountRegion.Add(loginView);
        }
    }
}
