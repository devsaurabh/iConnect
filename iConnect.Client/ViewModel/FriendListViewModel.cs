using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using iConnect.Data.ApplicationServices.Contract;
using iConnect_Client.Views;

namespace iConnect_Client.ViewModel
{
    public class FriendViewModel : ViewModelBase
    {
        public ICommand StartChatCommand { get; internal set; }

        public string Alias { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }

        public FriendViewModel()
        {
            StartChatCommand = new RelayCommand<string>(StartChatExecute);
        }

        private void StartChatExecute(string userName)
        {
            var cw = new ChatWindow(userName);
            
            cw.Show();
            //MessageBox.Show(userName);
        }
    }

    public class FriendListViewModel : ViewModelBase
    {

        public FriendListViewModel(IUserService userService)
        {
            FriendList = new ObservableCollection<FriendViewModel>();
            var resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri("../Resources/ImageResources.xaml",UriKind.RelativeOrAbsolute);
            var defaultImage = (ImageSource) resourceDictionary["UserDefaultImage"];
            
            
            foreach (var friend in  userService.GetAllUsers())
            {
                FriendList.Add(new FriendViewModel
                {
                    Alias =friend.Alias,
                    AvatarUrl = string.IsNullOrWhiteSpace(friend.AvatarUrl) ? defaultImage.ToString() : friend.AvatarUrl,
                    UserName = friend.EmailAddress
                });    
            }

            
        }

        public ObservableCollection<FriendViewModel> FriendList;
        
    }
}
