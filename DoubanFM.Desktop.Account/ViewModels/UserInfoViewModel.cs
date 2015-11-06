using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
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
                this._loginResult.DoubanUserId,
                this._loginResult.AccessToken,
                this._loginResult.ExpireIn);

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



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.User = null;
        }
    }
}
