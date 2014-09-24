using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #region Private Members

        private readonly ChatHelper _chatHelper;
        private readonly UserViewModel _ownerUser;
        private readonly UserViewModel _clientUser;

        #endregion

        #region Commands

        public ICommand StartChatCommand { get; internal set; }
        public ICommand CloseCommand { get; internal set; }
        public ICommand DragCommand { get; internal set; }

        #endregion

        #region Public Members

        public ObservableCollection<MessageViewModel> Messages { get; set; }

        #endregion

        #region Ctor

        public ChatViewModel(IUserService userService, string owner, UserViewModel client)
        {
            var user = userService.GetUser(owner);
            _ownerUser = new UserViewModel
            {
                UserName = user.EmailAddress,
                Alias = user.Alias,
                AvatarImage =
                    string.IsNullOrWhiteSpace(user.AvatarUrl) ? HelperFunctions.GetDefaultImage(user.EmailAddress) : user.AvatarUrl,
                UserId = user.UserId
            };
            _clientUser = client;
            Messages = new ObservableCollection<MessageViewModel>();
            StartChatCommand = new RelayCommand<string>(SendMessageExecute);
            DragCommand = new RelayCommand(DragExecute);
            CloseCommand = new RelayCommand(CloseExecute);
            _chatHelper = ChatHelper.Instance;
            _chatHelper.PrivateMessage += ChatHelperOnPrivateMessage;
        }

        private void CloseExecute()
        {
            
        }

        private void DragExecute()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

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
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() => Messages.Add(messageModel)));

            }
        }

        private void SendMessageExecute(string message)
        {
            _chatHelper.SendPrivateAsync(_clientUser.UserName, message);
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

        //private void UpdateMessages(MessageViewModel message)
        //{
        //    Messages.Add(message);
        //} 

        private ObservableCollection<MessageViewModel> GetList()
        {
            var list = new List<MessageViewModel>
            {
                new MessageViewModel
                {
                    Alias = "Saurabh",
                    AvatarUrl = Utilities.HelperFunctions.GetDefaultImage("saurabh.singh@cardinalts.com"),
                    IsOwnerMessage = true,
                    Message = "Greetings",
                    UserName = "saurabh.singh@cardinalts.com"
                },
                new MessageViewModel
                {
                    Alias = "Manpreet",
                    AvatarUrl = Utilities.HelperFunctions.GetDefaultImage("manpreet.singh@cardinalts.com"),
                    IsOwnerMessage = false,
                    Message = "Greetings",
                    UserName = "manpreet.singh@cardinalts.com"
                }
            };
            return new ObservableCollection<MessageViewModel>(list);
        }

        #endregion
    }

    public class MessageViewModel : ViewModelBase
    {
        private bool _isOwnerMessage;

        public string Alias { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }

        public bool IsOwnerMessage
        {
            get { return _isOwnerMessage; }
            set { _isOwnerMessage = value; RaisePropertyChanged("IsOwnerMessage"); }
        }
    }
}