namespace iConnect.Server.Framework.Hubs
{
    public interface IChatHub
    {
        void Connect(string userName);
        void Disconnect(string userName);
        void SendPrivateMessage(string userName, string message);
        void BroadcastToAll(string message);
        void PingUser(string userName);
        //List<User> GetAllUsers();
    }
}
