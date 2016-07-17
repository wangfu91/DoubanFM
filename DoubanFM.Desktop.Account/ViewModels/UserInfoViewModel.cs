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

        private readonly IEventAggregator _eventAggregator;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;

        private readonly LoginResult _loginResult;
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
            User userInfo;
            try
            {
                userInfo = await this._userService.GetUserInfo(_loginResult.DoubanUserId, _loginResult.AccessToken);
            }
            catch
            {
                userInfo = null;
            }

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
