using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DoubanFM.Desktop.Account.ViewModels
{
    public class UserInfoViewModel : ViewModelBase
    {

        private IEventAggregator _eventAggregator;
        private ILoginService _loginService;
        private IUserService _userService;

        private LoginResult _loginResult;
        private User _user;

        public ICommand LogOffCommand { get; set; }

        public UserInfoViewModel(
            IEventAggregator eventAggregator,
            ILoginService loginService,
            IUserService userService,
            LoginResult loginResult)
        {
            this._eventAggregator = eventAggregator;
            this._loginService = loginService;
            this._userService = userService;
            this._loginResult = loginResult;

            this.LogOffCommand = new DelegateCommand(LogOff);

            GetUserInfo().ConfigureAwait(false);
        }

        private void LogOff()
        {
            _eventAggregator.GetEvent<UserStateChangedEvent>().Publish(null);
        }

        private async Task GetUserInfo()
        {
            var userInfo = await this._userService.GetUserInfo(
                this._loginResult.UserId,
                this._loginResult.Token,
                this._loginResult.Expire);

            if (userInfo != null)
            {
                this.User = userInfo;
            }
        }


        public User User
        {
            get { return _user; }
            set
            {
                if (value != _user)
                {
                    _user = value;
                    OnPropertyChanged(() => this.User);
                }
            }
        }
    }
}
