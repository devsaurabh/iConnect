using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iConnect_Client.Views;

namespace iConnect_Client
{
    public class Startup
    {
        [STAThread()]
        static void Main()
        {
            var app = new Application();
            
            var mainWindow = new FriendList();
            app.Run(mainWindow);
            
        }
    }
}
