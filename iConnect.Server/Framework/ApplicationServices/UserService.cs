using System;
using System.Collections.Generic;
using System.Linq;
using iConnect.Server.Framework.ApplicationServices.Contract;
using iConnect.Server.Framework.Data;
using iConnect.Server.Framework.Data.Model;

namespace iConnect.Server.Framework.ApplicationServices
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

        public List<User> GetAllUsers()
        {
            return _chatContext.Users.Where(usr => usr.IsActive).ToList();
        }

        public User GetUser(string userName)
        {
            return _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
        }

        public bool MarkActive(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsActive = true;
            _chatContext.SaveChanges();
            return true;
        }

        public bool MarkInactive(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsActive = false;
            _chatContext.SaveChanges();
            return true;
        }

        public bool MarkOnline(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsOnline = true;
            _chatContext.SaveChanges();
            return true;
        }

        public bool MarkOffline(string userName)
        {
            var user = _chatContext.Users.FirstOrDefault(usr => usr.EmailAddress == userName);
            if (user == null) return false;
            user.IsOnline = false;
            _chatContext.SaveChanges();
            return true;
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
