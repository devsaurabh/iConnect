using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using iConnect.Data.ApplicationServices.Contract;
using iConnect_Client.Utilities;
using iConnect_Client.Views;

namespace iConnect_Client.ViewModel
{
    public class UserViewModel 
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarImage { get; set; }
        public string Alias { get; set; }
    }

    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public ICommand LoginCommand { get; internal set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public LoginViewModel(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            LoginCommand = new RelayCommand<object>(LoginExecute);
        }

        private void LoginExecute(object passwordBox)
        {
            var passwordControl = (passwordBox as PasswordBox);
            if (passwordControl != null)
            {
                var password = passwordControl.Password;
                if (_authenticationService.Validate(UserName, password))
                {
                    var chatHelper = ChatHelper.Instance;
                    chatHelper.Login(UserName);
                    var window = new FriendList(_userService,UserName);
                    var mainWindow = Application.Current.Windows[0];
                    if(mainWindow!=null)
                        mainWindow.Close();

                    window.Show();
                }
            }

        }
    }
}
