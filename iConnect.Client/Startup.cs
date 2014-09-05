using System;
using System.Threading;
using System.Windows;
using iConnect_Client.Views;

namespace iConnect_Client
{
    public class Startup
    {
        private static Mutex _mutex;
        
        [STAThread()]
        static void Main()
        {
            bool isNew;
            _mutex = new Mutex(true,"iConnect",out isNew);
            if (!isNew)
            {
                Environment.Exit(0);
            }
            else
            {
                var app = new Application();
                var mainWindow = new MainWindow();
                app.Run(mainWindow);    
            }
            
        }
    }
}
