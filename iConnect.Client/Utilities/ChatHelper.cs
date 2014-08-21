using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNet.SignalR.Client;

namespace iConnect_Client.Utilities
{
    class ChatHelper
    {
        private const string HOST = "http://saurabh-singh/iConnect/signalr/hubs";

        public IHubProxy Proxy { get; private set; }

        public HubConnection Connection { get; private set; }

        public async Task ConnectAsync()
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

    }
}
