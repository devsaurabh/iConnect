using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iConnect.Data;
using iConnect.Data.Framework;
using Microsoft.AspNet.SignalR;
using WebMatrix.WebData;

namespace iConnect.Server.Framework.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        public static List<InternalUser> UserList = new List<InternalUser>();

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;
            

            using (var dbContext = new ChatContext())
            {
                var matchingUser = UserList.FirstOrDefault(u => u.ConnectionId == id);
                

                if (matchingUser == null)
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.EmailAddress == userName);
                    //UserList.Remove(UserList.FirstOrDefault(u => u.UserName == userName));
                    if (user != null)
                    {
                        user.IsOnline = true;
                        dbContext.SaveChanges();
                        //UserList.Remove(UserList.FirstOrDefault(u => u.UserName == userName));
                        UserList.Add(new InternalUser { ConnectionId = id, UserName = userName, Alias = user.Alias });
                        Clients.AllExcept(new[] { id }).onConnect(userName);
                    }
                }

               
                
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
                        //WebSecurity.Logout();
                        Clients.AllExcept(new[] {id}).onDisconnect(matchingUser.UserName);
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
                        WebSecurity.Logout();
                        Clients.AllExcept(new[] { id }).onDisconnect(matchingUser.UserName);
                    }
                }
            }
        }

        public void SendPrivateMessage(string userName, string message)
        {
            var fromUser = UserList.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var toUser = UserList.FirstOrDefault(u => u.UserName == userName);
            if (fromUser != null && toUser != null)
                Clients.Client(toUser.ConnectionId).onPrivate(fromUser.Alias, message);
        }

        public void BroadcastToAll(string message)
        {
            var id = Context.ConnectionId;
            var fromUser = UserList.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (fromUser != null)
                Clients.AllExcept(new[] { id }).onBroadcast(fromUser.UserName, message, fromUser.Alias);
        }

        public void PingUser(string userName)
        {
            var fromUser = UserList.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            var toUser = UserList.FirstOrDefault(u => u.UserName == userName);
            if (fromUser != null && toUser != null)
                Clients.Client(toUser.ConnectionId).onPing();
        }


        public async Task<bool> Update()
        {
           await Clients.All.onUpdate();
           return true;
        }

        public class InternalUser
        {
            public string UserName { get; set; }
            public string Alias { get; set; }
            public string ConnectionId { get; set; }
        }
    }
}
