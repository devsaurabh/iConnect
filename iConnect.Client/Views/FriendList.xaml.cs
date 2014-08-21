using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using iConnect.Data;
using iConnect.Data.ApplicationServices;
using iConnect.Data.ApplicationServices.Contract;
using iConnect_Client.ViewModel;

namespace iConnect_Client.Views
{
    public partial class FriendList
    {
        #region Private Members

        #endregion

        #region Ctor

        public FriendList()
        {
            InitializeComponent();
            var chatContext = new ChatContext();
            IUserService userService = new UserService(chatContext);
            var friendListView = new FriendListViewModel(userService);
            DataContext = friendListView;
            FriendListBox.ItemsSource = friendListView.FriendList;
        }
        
        #endregion

        #region Events
       

        //private void FriendListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{

        //    int index = FriendListBox.SelectedIndex;

        //    var listitem = (ListBoxItem)(FriendListBox.ItemContainerGenerator.ContainerFromIndex(index));

        //    var myContentPresenter = FindVisualChild<ContentPresenter>(listitem);

        //    var myDataTemplate = myContentPresenter.ContentTemplate;

        //    var myTextBlock = (TextBlock)myDataTemplate.FindName("textBlock", myContentPresenter);


        //    foreach (Window window in Application.Current.Windows)
        //    {
        //        var t = window.ToString();

        //        if (t == "ChatWindow")
        //        {
        //            var c = (ChatWindow)window;
        //            c.Close();
        //        }
        //    }

        //    var cw = new ChatWindow(myTextBlock.Text);
        //    var w = CenterWindowOnScreen();
        //    cw.Top = w.WindowTop;
        //    cw.Left = w.WindowLeft + 280 + 5;
        //    cw.Show();

        //}

        protected void OnDrag(object sender, EventArgs e)
        {
            DragMove();
        }

        protected void OnClose(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }


        #endregion

        #region Others

        //private TChildItem FindVisualChild<TChildItem>(DependencyObject obj) where TChildItem : DependencyObject
        //{

        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {

        //        var child = VisualTreeHelper.GetChild(obj, i);

        //        if (child != null && child is TChildItem)

        //            return (TChildItem)child;

        //        var childOfChild = FindVisualChild<TChildItem>(child);

        //        if (childOfChild != null)

        //            return childOfChild;
        //    }

        //    return null;

        //}

        private WindowTopAndLeft CenterWindowOnScreen()
        {
            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;
            double windowWidth = Width;
            double windowHeight = Height;

            var wtl = new WindowTopAndLeft
            {
                WindowLeft = (screenWidth / 2) - (windowWidth / 2),
                WindowTop = (screenHeight / 2) - (windowHeight / 2)
            };
            return wtl;
        }

        public struct WindowTopAndLeft
        {
            public double WindowLeft;
            public double WindowTop;
        }

        #endregion
    }
}
