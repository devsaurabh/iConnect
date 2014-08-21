using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.AspNet.SignalR.Client;

namespace iConnect_Client
{
    public partial class MainWindow
    {
        #region Private Members

        private const string HOST = "http://saurabh-singh/iConnect/signalr/hubs";

        #endregion

        #region Public Members

        public IHubProxy Proxy { get; set; }

        public HubConnection Connection { get; set; }

        public bool Active { get; set; }

        #endregion

        #region Ctor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private async void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = TextBox.Text;
                var paragraph = new Paragraph { TextAlignment = TextAlignment.Right };
                paragraph.Inlines.Add(text);
                await SendPrivate("SERVER", text);
                GetCallerParagraphBlock(text);
              
                //MessageList.Document.Blocks.Add();
                TextBox.Text = "";
            }
        }

        #endregion

        #region Private Methods

        private void GetCallerParagraphBlock(string text)
        {
            var paragraph = new Paragraph { TextAlignment = TextAlignment.Right, };
            paragraph.Inlines.Add(text);

            var paragraph1 = new Paragraph { TextAlignment = TextAlignment.Right };
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
           // RowGroup.Rows.Add(row);
        }

        private void GetServerParagraphBlock(string text)
        {
            var paragraph = new Paragraph { TextAlignment = TextAlignment.Left, };
            paragraph.Inlines.Add(text);
            var paragraph1 = new Paragraph { TextAlignment = TextAlignment.Left };
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
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => GetServerParagraphBlock(message)));
        }

        private async Task ConnectAsync()
        {
            Connection = new HubConnection(HOST);
            Proxy = Connection.CreateHubProxy("ChatHub");
           SubscribeToEvents();
            
            await Connection.Start();
            await Proxy.Invoke("connect","saurabh.singh@cardinalts.com");
        }

        private void SubscribeToEvents()
        {
            Proxy.On("onLoginFail", (string msg) => MessageBox.Show(msg));
        }

        private async Task SendPrivate(string userName,string message)
        {
            await Proxy.Invoke("sendPrivate",Environment.MachineName,userName,message);
        }

        #endregion
       
    }
}
