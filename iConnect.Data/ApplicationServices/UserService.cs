using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iConnect.Data.ApplicationServices.Contract;
using iConnect.Data.Model;

namespace iConnect.Data.ApplicationServices
{
    public class UserService : IUserService
    {
        #region Private Members

        private readonly ChatContext _chatContext;
        
        #endregion
        
        #region Ctor

        public UserService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var task = Task.Factory.StartNew(() => _chatContext.Users.Where(usr => usr.IsActive));
            await task;
            return task.Result;
        }

        IEnumerable<User> IUserService.GetAllUsers()
        {
            return _chatContext.Users.Where(usr => usr.IsActive);
        }

        public async Task<User> GetUserAsync(string userName)
        {
            var task =
                Task.Factory.StartNew(() => GetUser(userName));
            await task;
            return task.Result;
        }

        public User GetUser(string userName)
        {
            return _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
        }

        public async Task<bool> MarkActiveAsync(string userName)
        {
            var task = Task.Factory.StartNew(()=>MarkActive(userName));
            await task;
            return task.Result;
        }

        public bool MarkActive(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsActive = true;
            _chatContext.SaveChanges();
            return true;
        }

        public async Task<bool> MarkInactiveAsync(string userName)
        {
            var task = Task.Factory.StartNew(() => MarkInactive(userName));
            await task;
            return task.Result;
        }

        public bool MarkInactive(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsActive = false;
            _chatContext.SaveChanges();
            return true;
        }

        public async Task<bool> MarkOnlineAsync(string userName)
        {
            var task = Task.Factory.StartNew(() => MarkOnline(userName));
            await task;
            return task.Result;
        }

        public bool MarkOnline(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsOnline = true;
            _chatContext.SaveChanges();
            return true;
        }

        public async Task<bool> MarkOfflineAsync(string userName)
        {
            var task = Task.Factory.StartNew(() => MarkOffline(userName));
            await task;
            return task.Result;
        }

        public bool MarkOffline(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsOnline = false;
            _chatContext.SaveChanges();
            return true;
        }

        public async Task CreateUserAsync(User user)
        {
            var task = Task.Factory.StartNew(() => CreateUser(user));
            await task;
        }

        public void CreateUser(User user)
        {
            if (user != null)
            {
                user.IsActive = true;
                user.IsOnline = false;
                user.CreatedOn = DateTime.Now;
                user.ModifiedOn = DateTime.Now;
                _chatContext.Users.Add(user);
                _chatContext.SaveChanges();
            }

        }

        public Task DeleteUserAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return;
            _chatContext.Users.Remove(user);
            _chatContext.SaveChanges();
        } 

        #endregion
    }
}
