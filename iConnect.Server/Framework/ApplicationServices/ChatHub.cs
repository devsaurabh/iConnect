using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iConnect.Server.Framework.ApplicationServices.Contract;
using iConnect.Server.Framework.Data;
using Microsoft.AspNet.SignalR;

namespace iConnect.Server.Framework.ApplicationServices
{
    public class ChatHub : Hub, IChatHub
    {
        private const string SERVER_NAME = "SERVER";
        public static List<InternalUser> UserList = new List<InternalUser>();

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;
            using (var dbContext = new ChatContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.EmailAddress == userName);
                if (user == null)
                {
                    Clients.Caller.onLoginFail(ConstantStrings.INVALID_USER);
                    return;
                }

                user.IsOnline = true;
                dbContext.SaveChanges();
                UserList.Remove(UserList.FirstOrDefault(u => u.UserName == userName));
                UserList.Add(new InternalUser { ConnectionId = id, UserName = userName });
                Clients.All.onConnect(userName);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;
            var matchingUser = UserList.FirstOrDefault(user => user.ConnectionId == id);
            if (matchingUser != null)
            {
                using (var dbContext = new ChatContext())
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.EmailAddress == matchingUser.UserName);
                    if (user != null)
                    {
                        user.IsOnline = false;
                        dbContext.SaveChanges();
                        UserList.Remove(matchingUser);
                        Clients.All.onDisconnect(matchingUser.UserName);
                    }
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        public void Disconnect(string userName)
        {
            var id = Context.ConnectionId;
            var matchingUser = UserList.FirstOrDefault(user => user.ConnectionId == id);
            if (matchingUser != null)
            {
                using (var dbContext = new ChatContext())
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.EmailAddress == matchingUser.UserName);
                    if (user != null)
                    {
                        user.IsOnline = false;
                        dbContext.SaveChanges();
                        UserList.Remove(matchingUser);
                        Clients.All.onDisconnect(matchingUser.UserName);
                    }
                }
            }
        }

        public void SendPrivateMessage(string userName, string message)
        {
            var fromUser = UserList.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var toUser = UserList.FirstOrDefault(u => u.UserName == userName);
            if (fromUser != null && toUser != null)
                Clients.Client(toUser.ConnectionId).onPrivate(fromUser.UserName, message);
        }

        public void BroadcastToAll(string message)
        {
            var fromUser = UserList.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (fromUser != null)
                Clients.All.onBroadcast(fromUser.UserName, message);
        }

        public void PingUser(string userName)
        {
            var fromUser = UserList.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var toUser = UserList.FirstOrDefault(u => u.UserName == userName);
            if (fromUser != null && toUser != null)
                Clients.Client(toUser.ConnectionId).onPing();
        }

        public class InternalUser
        {
            public string UserName { get; set; }
            public string ConnectionId { get; set; }
        }
    }
}
