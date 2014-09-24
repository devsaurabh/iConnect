using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using iConnect.Data.ApplicationServices.Contract;
using iConnect_Client.Utilities;
using iConnect_Client.Views;

namespace iConnect_Client.ViewModel
{
    public class FriendViewModel : ViewModelBase
    {
        public ICommand StartChatCommand { get; internal set; }

        public string Alias { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string ParentUserName { get; set; }

        public FriendViewModel()
        {
            StartChatCommand = new RelayCommand<string>(StartChatExecute);
        }

        private void StartChatExecute(string userName)
        {
            var clientUser = new UserViewModel
            {
                UserName = UserName,
                Alias = Alias,
                AvatarImage = AvatarUrl,
                UserId = 0
            };
           
            WindowFactory.OpenChatWindow(ParentUserName,clientUser);
           
        }
    }

    public class FriendListViewModel : ViewModelBase
    {
        public FriendListViewModel(IUserService userService,string userName)
        {
            var allUsers = userService.GetAllUsers().ToList();
            
            var user = allUsers.FirstOrDefault(t => t.EmailAddress == userName);
            allUsers.Remove(user);


            var users = allUsers.OrderBy(t => t.IsOnline).Select(friend => new FriendViewModel
            {
                Alias = friend.Alias,
                AvatarUrl =
                    string.IsNullOrWhiteSpace(friend.AvatarUrl)
                        ? HelperFunctions.GetDefaultImage(friend.EmailAddress)
                        : friend.AvatarUrl,
                UserName = friend.EmailAddress,
                Status = friend.IsOnline ? HelperFunctions.GetOnlineImage() : null,
                ParentUserName = user.EmailAddress
            });

            FriendList = new ObservableCollection<FriendViewModel>(users);
        }

        public ObservableCollection<FriendViewModel> FriendList;
    }

    
}
