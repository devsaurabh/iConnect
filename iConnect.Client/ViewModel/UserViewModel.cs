using System;
using System.Threading;
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
        #region Private Members
        private readonly CancellationTokenSource _loginCancellationTokenSource;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ChatHelper _chatHelper;
        private string _message;
        private bool _isErrorMessage;
        private bool _isEnabled;
        #endregion

        #region Public Members

        public ICommand LoginCommand { get; internal set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged("Message"); }
        }
        public bool IsErrorMessage
        {
            get { return _isErrorMessage; }
            set { _isErrorMessage = value; RaisePropertyChanged("IsErrorMessage"); }
        }
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); } }

        #endregion

        #region Ctor

        public LoginViewModel(IUserService userService, IAuthenticationService authenticationService)
        {
            IsEnabled = true;
            _userService = userService;
            _authenticationService = authenticationService;
            _chatHelper = ChatHelper.Instance;
            SubscribeToEvents();
            LoginCommand = new RelayCommand<object>(LoginExecute);
            _loginCancellationTokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Private Methods

        private void ChatHelperOnLoginFailed(object sender, EventArgs eventArgs)
        {
            IsErrorMessage = true;
            Message = "Something went wrong. Please try again";
            _loginCancellationTokenSource.Cancel();
        }

        private async void LoginExecute(object passwordBox)
        {
            IsEnabled = false;
            Message = string.Empty;
            IsErrorMessage = false;

            var passwordControl = (passwordBox as PasswordBox);
            if (passwordControl != null)
            {
                var password = passwordControl.Password;
                var validationResult = await _authenticationService.Validate(UserName, password);
                if (validationResult)
                {
                    Message = "Connecting to server";
                    await _chatHelper.EstablishConnection();
                    Message = "Signing In";
                    await _chatHelper.Login(UserName);
                    if (_chatHelper.IsLoggedIn)
                    {
                        var window = new FriendList(_userService, UserName);
                        var mainWindow = Application.Current.Windows[0];
                        if (mainWindow != null)
                            mainWindow.Close();
                        window.Show();
                    }
                    else
                    {
                        IsErrorMessage = true;
                        Message = "Unable to Connect...";
                        IsEnabled = true;
                    }
                }
                else
                {
                    IsErrorMessage = true;
                    Message = "Invalid Credentials";
                    IsEnabled = true;
                }
            }
        }

        private void SubscribeToEvents()
        {
            _chatHelper.LoginFailed += ChatHelperOnLoginFailed;
        }

        #endregion
    }
}
