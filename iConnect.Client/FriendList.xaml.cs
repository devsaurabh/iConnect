using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iConnect.Data;
using iConnect.Data.ApplicationServices;
using iConnect.Data.Model;

namespace iConnect_Client
{
    /// <summary>
    /// Interaction logic for FriendList.xaml
    /// </summary>
    public partial class FriendList : Window
    {
        public FriendList()
        {
            InitializeComponent();
            FriendListBox.ItemsSource = GetUserList();
        }

        private IEnumerable<User> GetUserList()
        {
            var dataContext = new ChatContext();
            
            var users = new UserService(dataContext).GetAllUsers();
            return users;
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
