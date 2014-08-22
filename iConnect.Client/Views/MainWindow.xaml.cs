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


        #region Private Methods

        //private void GetCallerParagraphBlock(string text)
        //{
        //    var paragraph = new Paragraph { TextAlignment = TextAlignment.Right, };
        //    paragraph.Inlines.Add(text);

        //    var paragraph1 = new Paragraph { TextAlignment = TextAlignment.Right };
        //    paragraph1.Inlines.Add("Me");


        //    var row = new TableRow();
        //    var contentCell = new TableCell
        //    {
        //        Padding = new Thickness(5),
        //        Background = Brushes.White,
        //        BorderThickness = new Thickness(0.5),
        //        BorderBrush = Brushes.Transparent,
        //    };


        //    contentCell.Blocks.Add(paragraph1);
        //    contentCell.Blocks.Add(paragraph);
        //    row.Cells.Add(contentCell);
        //   // RowGroup.Rows.Add(row);
        //}

        //private void GetServerParagraphBlock(string text)
        //{
        //    var paragraph = new Paragraph { TextAlignment = TextAlignment.Left, };
        //    paragraph.Inlines.Add(text);
        //    var paragraph1 = new Paragraph { TextAlignment = TextAlignment.Left };
        //    paragraph1.Inlines.Add("Server");
        //    var row = new TableRow();
        //    var contentCell = new TableCell
        //    {
        //        Padding = new Thickness(5),
        //        Background = Brushes.White,

        //        BorderThickness = new Thickness(0.5),

        //        BorderBrush = Brushes.Transparent,
        //    };
        //    contentCell.Blocks.Add(paragraph1);
        //    contentCell.Blocks.Add(paragraph);
        //    row.Cells.Add(contentCell);
        //   // RowGroup.Rows.Add(row);
        //}

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var task = new Task(() => ConnectAsync());
        //    task.Start();
        //}

        //private void PostServerData(string message)
        //{
        //    Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => GetServerParagraphBlock(message)));
        //}

        //private async Task ConnectAsync()
        //{
        //    Connection = new HubConnection(HOST);
        //    Proxy = Connection.CreateHubProxy("ChatHub");
        //   SubscribeToEvents();
            
        //    await Connection.Start();
        //    await Proxy.Invoke("connect","saurabh.singh@cardinalts.com");
        //}

        //private void SubscribeToEvents()
        //{
        //    Proxy.On("onLoginFail", (string msg) => MessageBox.Show(msg));
        //}

        //private async Task SendPrivate(string userName,string message)
        //{
        //    await Proxy.Invoke("sendPrivate",Environment.MachineName,userName,message);
        //}

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
