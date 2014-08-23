using System;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using iConnect.Data.ApplicationServices.Contract;
using iConnect_Client.Utilities;

namespace iConnect_Client.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly ChatHelper _chatHelper;
        private readonly UserViewModel _ownerUser;
        private readonly UserViewModel _clientUser;

        public ICommand StartChatCommand { get; internal set; }
        
        public ObservableCollection<MessageViewModel> Messages { get; set; }

        public ChatViewModel(IUserService userService,string owner,UserViewModel client)
        {
            var user = userService.GetUser(owner);
            _ownerUser = new UserViewModel
            {
                UserName = user.EmailAddress,
                Alias = user.Alias,
                AvatarImage =
                    string.IsNullOrWhiteSpace(user.AvatarUrl) ? HelperFunctions.GetDefaultImage() : user.AvatarUrl,
                UserId = user.UserId
            };
            _clientUser = client;
            Messages = new ObservableCollection<MessageViewModel>();
            StartChatCommand = new RelayCommand<string>(SendMessageExecute);
            _chatHelper = ChatHelper.Instance;
            _chatHelper.PrivateMessage += ChatHelperOnPrivateMessage;
            
        }

        private void ChatHelperOnPrivateMessage(object sender, MessageArgs messageArgs)
        {
            if (messageArgs.UserName == _clientUser.UserName)
            {
                var messageModel = new MessageViewModel
                {
                    Alias = _clientUser.Alias,
                    AvatarUrl = _clientUser.AvatarImage,
                    IsOwnerMessage = false,
                    Message = messageArgs.Message,
                    UserName = _clientUser.UserName
                };
               Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => UpdateMessages(messageModel)));
               
            }
        }

        private void SendMessageExecute(string message)
        {
            _chatHelper.SendPrivate(_clientUser.UserName, message);
            var messageModel = new MessageViewModel
            {
                Alias = _ownerUser.Alias,
                AvatarUrl = _ownerUser.AvatarImage,
                IsOwnerMessage = true,
                Message = message,
                UserName = _ownerUser.UserName
            };
            Messages.Add(messageModel);
        }

        private void UpdateMessages(MessageViewModel message)
        {
            Messages.Add(message);
        }
    }

    public class MessageViewModel : ViewModelBase
    {
        public string Alias { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public bool IsOwnerMessage { get; set; }
    }
}