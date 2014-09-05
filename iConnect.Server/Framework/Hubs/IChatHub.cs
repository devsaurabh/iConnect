using System.Threading.Tasks;

namespace iConnect.Server.Framework.Hubs
{
    public interface IChatHub
    {
        Task<bool> Connect(string userName);
        void Disconnect(string userName);
        void SendPrivateMessage(string userName, string message);
        void BroadcastToAll(string message);
        void PingUser(string userName);
        //List<User> GetAllUsers();
    }
}
