using System;
using System.Threading.Tasks;
using iConnect.Data.ApplicationServices.Contract;

namespace iConnect.Data.ApplicationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> Validate(string userName, string password)
        {
            var loginTask = Task.Run(() =>
            {
                var user = _userService.GetUser(userName);
                return user != null && string.Compare(password, user.Password, StringComparison.Ordinal) == 0;
            });
            await loginTask;
            return loginTask.Result;
        }
    }
}
