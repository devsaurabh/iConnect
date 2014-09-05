using System.Collections.Generic;
using System.Threading.Tasks;
using iConnect.Data.Model;

namespace iConnect.Data.ApplicationServices.Contract
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        IEnumerable<User> GetAllUsers();

        Task<User> GetUserAsync(string userName);
        User GetUser(string userName);

        Task<bool> MarkActiveAsync(string userName);
        bool MarkActive(string userName);

        Task<bool> MarkInactiveAsync(string userName);
        bool MarkInactive(string userName);

        Task<bool> MarkOnlineAsync(string userName);
        bool MarkOnline(string userName);

        Task<bool> MarkOfflineAsync(string userName);
        bool MarkOffline(string userName);

        Task CreateUserAsync(User user);
        void CreateUser(User user);

        Task DeleteUserAsync(string userName);
        void DeleteUser(string userName);
    }
}
