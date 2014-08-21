using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace iConnect_Client.Views
{
    public partial class ChatWindow
    {
        #region Private Members

        private const string HOST = "http://saurabh-singh/iConnect/signalr/hubs";

        #endregion

        #region Public Members

        public IHubProxy Proxy { get; set; }

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        public string UserName { get; set; }

        public override string ToString()
        {
            return "ChatWindow";
        }

        #endregion

        #region Ctor

        public ChatWindow(string name)
        {
            InitializeComponent();
            UserName = name;

            TitleBar.Title = name;
        }

        #endregion

        #region Events

        private async void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = TextBox.Text;
                var paragraph = new Paragraph {TextAlignment = TextAlignment.Right};
                paragraph.Inlines.Add(text);
                await SendPrivate("sysadmin@cardinalts.com", text);
                GetCallerParagraphBlock(text);

                //MessageList.Document.Blocks.Add();
                TextBox.Text = "";
            }
        }

        protected void OnDrag(object sender, EventArgs e)
        {
            DragMove();
        }

        protected void OnClose(object sender, EventArgs e)
        {
            Close();
        }

        private void chatWindow_Activated(object sender, EventArgs e)
        {
            TextBox.Focus();
        }

        private void chatWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Private Methods

        private void GetCallerParagraphBlock(string text)
        {
            var paragraph = new Paragraph {TextAlignment = TextAlignment.Right,};
            paragraph.Inlines.Add(text);

            var paragraph1 = new Paragraph {TextAlignment = TextAlignment.Right};
            paragraph1.Inlines.Add("Me");


            var row = new TableRow();
            var contentCell = new TableCell
            {
                Padding = new Thickness(5),
                Background = Brushes.White,
                BorderThickness = new Thickness(0.5),
                BorderBrush = Brushes.Transparent,
            };


            contentCell.Blocks.Add(paragraph1);
            contentCell.Blocks.Add(paragraph);
            row.Cells.Add(contentCell);
            RowGroup.Rows.Add(row);

            //below code is for testing only

            //test.Text = text;


        }

        private void GetServerParagraphBlock(string text)
        {
            var paragraph = new Paragraph {TextAlignment = TextAlignment.Left,};
            paragraph.Inlines.Add(text);
            var paragraph1 = new Paragraph {TextAlignment = TextAlignment.Left};
            paragraph1.Inlines.Add("Server");
            var row = new TableRow();
            var contentCell = new TableCell
            {
                Padding = new Thickness(5),
                Background = Brushes.White,

                BorderThickness = new Thickness(0.5),

                BorderBrush = Brushes.Transparent,
            };
            contentCell.Blocks.Add(paragraph1);
            contentCell.Blocks.Add(paragraph);
            row.Cells.Add(contentCell);
            // RowGroup.Rows.Add(row);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var task = new Task(() => ConnectAsync());
            task.Start();
        }

        private void PostServerData(string message)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action) (() => GetServerParagraphBlock(message)));
        }

        private async Task ConnectAsync()
        {
            Connection = new HubConnection(HOST);
            Proxy = Connection.CreateHubProxy("ChatHub");
            SubscribeToEvents();

            await Connection.Start();
            await Proxy.Invoke("connect", "saurabh.singh@cardinalts.com");
        }

        private void SubscribeToEvents()
        {
            Proxy.On("onLoginFail", (string msg) => MessageBox.Show(msg));
        }

        private async Task SendPrivate(string userName, string message)
        {
            await Proxy.Invoke("SendPrivateMessage", userName, message);
        }

        #endregion
    }
}
