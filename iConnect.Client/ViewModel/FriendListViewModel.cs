using System.Collections.ObjectModel;
using System.Linq;
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

            var cw = new ChatWindow(ParentUserName,clientUser);
            cw.Show();
        }
    }

    public class FriendListViewModel : ViewModelBase
    {
        public FriendListViewModel(IUserService userService,string userName)
        {
            FriendList = new ObservableCollection<FriendViewModel>();
            //Init(userService);

            var allUsers = userService.GetAllUsers();
            var user = allUsers.FirstOrDefault(t => t.EmailAddress == userName);
            allUsers.Remove(user);

            foreach (var friend in allUsers.OrderBy(t => t.IsOnline))
            {
                FriendList.Add(new FriendViewModel
                {
                    Alias = friend.Alias,
                    AvatarUrl =
                        string.IsNullOrWhiteSpace(friend.AvatarUrl)
                            ? HelperFunctions.GetDefaultImage()
                            : friend.AvatarUrl,
                    UserName = friend.EmailAddress,
                    Status = friend.IsOnline?HelperFunctions.GetOnlineImage():null,
                    ParentUserName = user.EmailAddress
                });
            }

            
        }

        public ObservableCollection<FriendViewModel> FriendList;

        private void Init(IUserService userService)
        {
            var chatHelper = ChatHelper.Instance;
            var user = userService.GetUser("saurabh.singh@cardinalts.com");
            if (user != null)
            {
                chatHelper.Login(user.EmailAddress);
                //_userViewModel.Alias = user.Alias;
               // _userViewModel.AvatarImage = string.IsNullOrWhiteSpace(user.AvatarUrl)
                //    ? HelperFunctioncs.GetDefaultImage()
                //    : user.AvatarUrl;
            }
        }
    }

    
}
