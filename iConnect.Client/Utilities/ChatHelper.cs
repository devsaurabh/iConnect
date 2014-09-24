using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using iConnect.Data.Framework;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace iConnect_Client.Utilities
{
    public class ChatHelper
    {
        #region Private Members

        private static readonly Lazy<ChatHelper> PrivateInstance = new Lazy<ChatHelper>(() => new ChatHelper());
        private readonly string _chatServerAddress;
        private CancellationTokenSource _cancellationToken;

        #endregion

        #region Ctor

        private ChatHelper()
        {
            _chatServerAddress = ConfigurationManager.AppSettings.Get("ChatServer");
            IsLoggedIn = false;
        }
        
        #endregion

        #region Public Members

        public string Host { get { return _chatServerAddress; } }

        public IHubProxy Proxy { get; private set; }

        public HubConnection Connection { get; private set; }

        public bool IsLoggedIn { get; private set; }

        public event EventHandler<MessageArgs> PrivateMessage;

        protected async virtual void OnPrivateMessage(MessageArgs e)
        {
            EventHandler<MessageArgs> handler = PrivateMessage;
            if (handler != null) handler(this, e);
        }

        public event EventHandler LoginFailed;

        public event EventHandler<ConnectionArgs> ConnectionStateChanged;

        #endregion

        #region Public Methods

        public static ChatHelper Instance
        {
            get { return PrivateInstance.Value; }
        }

        public async Task EstablishConnectionAsync()
        {
            if (Connection != null && Connection.State == ConnectionState.Connected) return;
            Connection = new HubConnection(Host);
        
            Proxy = Connection.CreateHubProxy("ChatHub");
            SubscribeToEvents();
            await Connection.Start(new LongPollingTransport());
            //Connection.Transport  = ;
        }

        public async Task LoginAsync(string userName)
        {
            IsLoggedIn = true;
            if (Connection.State == ConnectionState.Connected)
            {
                try
                {
                    await Proxy.Invoke("Connect", userName);
                    //IsLoggedIn = true;
                }
                catch (Exception)
                {
                    IsLoggedIn = false;
                }
            }
            else
            {
                IsLoggedIn = false;
                throw new Exception(ExceptionConstants.NotConnected);
            }
        }

        public async Task SendPrivateAsync(string userName, string message)
        {
            await Proxy.Invoke("SendPrivateMessage", userName, message);
        }

        private static bool AuthenticateUser(string user, string password, out Cookie authCookie)
        {
            var request = WebRequest.Create("https://www.contoso.com/RemoteLogin") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = new CookieContainer();

            var credentials = "UserName=" + user + "&Password=" + password;
            var bytes = System.Text.Encoding.UTF8.GetBytes(credentials);
            request.ContentLength = bytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                authCookie = response.Cookies[FormsAuthentication.FormsCookieName];
            }

            if (authCookie != null)
            {
                return true;
            }
            return false;
        }
        
        #endregion

        #region Private Methods

        private void SubscribeToEvents()
        {
            Proxy.On("onLoginFail", (string msg) => OnLoginFailed(msg));
            Proxy.On("onPrivate", (string user, string msg) => PrivateMessage.Invoke(this, new MessageArgs(user, msg)));
            //Connection.StateChanged +=
            //    change => ConnectionStateChanged.Invoke(this, new ConnectionArgs {State = change.NewState});
        }

        private void OnLoginFailed(string msg)
        {
            IsLoggedIn = false;
        }

        #endregion
    }

    public class MessageArgs : EventArgs
    {
        public MessageArgs(string userName,string message)
        {
            UserName = userName;
            Message = message;
        }
        public string UserName { get; set; }
        public string Message { get; set; }
    }

    public class ConnectionArgs : EventArgs
    {
        public ConnectionState State;
    }

   
    
}
