using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using iConnect.Data.Framework;
using Microsoft.AspNet.SignalR.Client;

namespace iConnect_Client.Utilities
{
    public delegate void MessageEventHandler(Object sender, MessageArgs e);
    class ChatHelper
    {
        public event MessageEventHandler PrivateMessage;
        public event EventHandler LoginFailed;

        protected virtual void OnLoginFailed()
        {
            EventHandler handler = LoginFailed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnPrivateMessage(MessageArgs e)
        {
            MessageEventHandler handler = PrivateMessage;
            if (handler != null) handler(this, e);
        }

        #region Private Members

        private static readonly Lazy<ChatHelper> PrivateInstance = new Lazy<ChatHelper>(() => new ChatHelper());
        private readonly string _chatServerAddress;

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

        #endregion

        #region Public Methods

        public static ChatHelper Instance
        {
            get
            {
                return PrivateInstance.Value;
            }
        }

        public async Task EstablishConnection()
        {
            if (Connection==null || Connection.State == ConnectionState.Disconnected)
            {

                Connection = new HubConnection(Host);
                Proxy = Connection.CreateHubProxy("ChatHub");
                SubscribeToEvents();
                await Connection.Start();
                
            }
            
        }

        public void Login(string userName)
        {
            if (Connection.State == ConnectionState.Connected)
            {
                Proxy.Invoke("Connect", userName);
                IsLoggedIn = true;
            }
            else
            {
                throw new Exception(ExceptionConstants.NotConnected);
            }
        }

        public async Task SendPrivate(string userName, string message)
        {
            await Proxy.Invoke("SendPrivateMessage", userName, message);
        }
        
        #endregion

        #region Private Methods

        private void SubscribeToEvents()
        {
            Proxy.On("onLoginFail", (string msg) => OnLoginFailed());
            Proxy.On("onPrivate", (string user,string msg) => OnPrivateMessage(new MessageArgs(user,msg)));
        }

        //private void PrivateReceived(string user, string msg)
        //{
        //    var handler = PrivateMessage;
        //    if(handler!=null)
        //        handler(this,new MessageArgs{Message = msg,UserName = user});
        //}

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
}
