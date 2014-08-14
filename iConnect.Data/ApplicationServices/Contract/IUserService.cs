using System.Collections.Generic;
using iConnect.Data.Model;

namespace iConnect.Data.ApplicationServices.Contract
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User GetUser(string userName);
        bool MarkActive(string userName);
        bool MarkInactive(string userName);
        bool MarkOnline(string userName);
        bool MarkOffline(string userName);
        void CreateUser(User user);
        void DeleteUser(string userName);
    }
}
