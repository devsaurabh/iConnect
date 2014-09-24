using iConnect.Data;
using iConnect.Data.ApplicationServices;
using iConnect_Client.ViewModel;
using System;

namespace iConnect_Client.Views
{
    public partial class ChatWindow
    {
        public static bool IsWindowOpen { get; private set; }
        public static string WindowName { get; private set; }

        #region Ctor

        public ChatWindow(string parentUserName,UserViewModel client)
        {
            var userService = new UserService(new ChatContext());
            InitializeComponent();
            var model = new ChatViewModel(userService, parentUserName, client);
            DataContext = model;
            lstChat.ItemsSource = model.Messages;
            TitleBar.Title = client.Alias;
            IsWindowOpen = true;
            WindowName = parentUserName;
        }

        #endregion

        #region Events

        protected void OnDrag(object sender, EventArgs e)
        {
            DragMove();
        }

        protected void OnClose(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
