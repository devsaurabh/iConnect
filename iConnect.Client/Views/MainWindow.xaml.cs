using System;
using System.Windows;
using iConnect.Data;
using iConnect.Data.ApplicationServices;
using iConnect_Client.ViewModel;

namespace iConnect_Client.Views
{
    public partial class MainWindow
    {
        #region Ctor

        public MainWindow()
        {
            InitializeComponent();
            var context = new ChatContext();
            var userService = new UserService(context);
            var authenticationService = new AuthenticationService(userService);
            var loginViewModel = new LoginViewModel(userService, authenticationService);
            DataContext = loginViewModel;
        }

        #endregion

        private void TitleBar_OnDrag(object sender, EventArgs e)
        {
            DragMove();
        }

        private void TitleBar_OnClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
