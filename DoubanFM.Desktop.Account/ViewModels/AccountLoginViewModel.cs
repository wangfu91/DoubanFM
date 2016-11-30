using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using DoubanFM.Desktop.Infrastructure.Extension;
using Prism.Commands;
using Prism.Events;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DoubanFM.Desktop.Account.ViewModels
{
    public class AccountLoginViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILoginService _loginService;
        private IUserService _userService;
        private ICredentialManageService _credentialStorageService;

        private string _userEmail;

        private LoginResult _loginResult;

        public AccountLoginViewModel(
            IEventAggregator eventAggregator,
            ILoginService loginService,
            IUserService userService,
            ICredentialManageService credentialStorageService
            )
        {
            this._eventAggregator = eventAggregator;
            this._loginService = loginService;
            this._userService = userService;
            this._credentialStorageService = credentialStorageService;

            LoginCommand = DelegateCommand<object>.FromAsyncHandler(LoginAsync);
        }

        private async Task LoginAsync(object obj)
        {
            var passwordBox = obj as PasswordBox;

            var password = passwordBox?.SecurePassword;
            if (!string.IsNullOrWhiteSpace(password?.ToString()))
            {
                _loginResult = await _loginService.LoginWithEmail(UserEmail, password.ConvertToUnsecureString());
                if (!string.IsNullOrEmpty(_loginResult.AccessToken))
                {
                    _eventAggregator.GetEvent<UserStateChangedEvent>().Publish(_loginResult);
                    await _credentialStorageService.SaveCredentialAsync(_loginResult);
                }
                else
                {
                    //TODO: Handle Login Errors.
                }
            }
        }

        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                if (value != _userEmail)
                {
                    _userEmail = value;
                    OnPropertyChanged(() => this.UserEmail);
                }
            }
        }

        public ICommand LoginCommand { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
