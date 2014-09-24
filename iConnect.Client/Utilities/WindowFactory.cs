using System.Collections.Generic;
using System.Linq;
using System.Windows;
using iConnect_Client.ViewModel;
using iConnect_Client.Views;

namespace iConnect_Client.Utilities
{
    public class WindowFactory
    {
        //private static readonly List<string> OpenWindows = new List<string>();
        private static readonly List<Window> OpenWindows = new List<Window>();
        
        public static void OpenChatWindow(string userName,UserViewModel client)
        {
            Window window = null;
            window = OpenWindows.FirstOrDefault(t => t.Title == userName);
            if (window!=null)
            {
                window.Focus();
            }
            else
            {
                window = new ChatWindow(userName, client);
                OpenWindows.Add(window);
                window.Show();
            }


        }

        public static void CloseChatWindow(string userName)
        {
            Window window = null;
            window = OpenWindows.FirstOrDefault(t => t.Title == userName);
            if (window == null) return;
            OpenWindows.Remove(window);
            window.Close();
        }
    }
}
