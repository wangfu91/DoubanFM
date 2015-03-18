using DoubanFM.Desktop.API.Models;
using DoubanFM.Desktop.API.Services;
using DoubanFM.Desktop.Infrastructure;
using DoubanFM.Desktop.Infrastructure.Events;
using DoubanFM.Desktop.Infrastructure.Extension;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DoubanFM.Desktop.Account.ViewModels
{
    public class AccountLoginViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private ILoginService _loginService;
        private IUserService _userService;

        private string _userEmail;

        private LoginResult _loginResult;

        public AccountLoginViewModel(
            IEventAggregator eventAggregator,
            ILoginService loginService,
            IUserService userService
            )
        {
            this._eventAggregator = eventAggregator;
            this._loginService = loginService;
            this._userService = userService;

            LoginCommand = new DelegateCommand<object>(Login);

			//Instead manual type in email and password everytime, here add a auto fire logic, just for test.
			//AutoFire();
        }

		/// <summary>
		/// Auto fire UserLoggedIn event, for test purposes only.
		/// </summary>
		//private void AutoFire()
		//{
		//	this.UserEmail = "wangfu91@hotmail.com";
		//	var passwordBox = new PasswordBox();
		//	passwordBox.Password = "wf19912012";

		//	this.LoginCommand.Execute(passwordBox);
		//}

        private async void Login(object obj)
        {
            var passwordBox = obj as PasswordBox;
            if (passwordBox == null) return;

            var password = passwordBox.SecurePassword;
            if (!string.IsNullOrWhiteSpace(password.ToString()))
            {
                _loginResult = await _loginService.LoginWithEmail(UserEmail, password.ConvertToUnsecureString());
                if (_loginResult.R == "0")
                {
                    _eventAggregator.GetEvent<UserLoggedInEvent>().Publish(_loginResult);
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

	}
}
