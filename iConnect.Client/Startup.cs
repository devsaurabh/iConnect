using System;
using System.Windows;
using iConnect_Client.Utilities;
using iConnect_Client.Views;

namespace iConnect_Client
{
    public class Startup
    {
        [STAThread()]
        static void Main()
        {
            var chatHelper = ChatHelper.Instance;
            chatHelper.EstablishConnection().Wait();
            var app = new Application();
            
            var mainWindow = new MainWindow();
            app.Run(mainWindow);
            
        }
    }
}
