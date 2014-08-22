using System;
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

        public bool Validate(string userName, string password)
        {
            var user = _userService.GetUser(userName);
            return user != null && string.Compare(password, user.Password, StringComparison.Ordinal) == 0;
        }
    }
}
