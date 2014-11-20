using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using iConnect.Data;
using iConnect.Data.ApplicationServices;
using iConnect.Data.ApplicationServices.Contract;
using iConnect.Data.Model;
using iConnect.Server.Framework.Emoctions;
using iConnect.Server.ViewModels;
using WebMatrix.WebData;
using iConnect.Server.Filters;

namespace iConnect.Server.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class ServerController : Controller
    {
        #region Private Members

        private readonly IUserService _userService;

        #endregion

        #region Ctor

        public ServerController()
        {
            var parser = new EmoctionParser();
            parser.PrepareEmoctions();
            var chatContext = new ChatContext();
            _userService = new UserService(chatContext);
            //_authenticationsService = new AuthenticationService(_userService);
        }
        
        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chat()
        {
            var allUsers = _userService.GetAllUsers();
            var model = allUsers.Select(usr => new UserViewModel
            {
                UserId = usr.UserId,
                UserName = usr.EmailAddress,
                FirstName = usr.FirstName,
                MiddleName = usr.MiddleName,
                LastName = usr.LastName,
                Alias = usr.Alias,
                IsOnline = usr.IsOnline.HasValue && usr.IsOnline.Value,
                RegisteredOn = usr.CreatedOn.Value
            }).OrderBy(u => u.RegisteredOn).ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetUserList()
        {
            var allUsers = _userService.GetAllUsers();
            var model = allUsers.Select(usr => new UserViewModel
            {
                UserId = usr.UserId,
                UserName = usr.EmailAddress,
                FirstName = usr.FirstName,
                MiddleName = usr.MiddleName,
                LastName = usr.LastName,
                Alias = usr.Alias,
                IsOnline = usr.IsOnline.HasValue && usr.IsOnline.Value,
                RegisteredOn = usr.CreatedOn.Value
            }).OrderBy(u => u.RegisteredOn).ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Create(UserViewModel model)
        {
            var user = new User
            {
                Alias = model.Alias,
                AvatarUrl = string.Empty,
                EmailAddress = model.UserName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                IsActive = true
            };

            if (!WebSecurity.UserExists(model.UserName))
            {
                //WebSecurity.CreateUserAndAccount(model.UserName, "123456", new { FirstName = model.FirstName, LastName = model.LastName, Alias = model.Alias, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, IsActive = true, IsOnline = false });
                WebSecurity.CreateUserAndAccount(model.UserName, "123456");
                Roles.AddUserToRole(model.UserName, model.UserType);
                _userService.UpdateUser(user);
            }

            return RedirectToAction("GetUserList");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string userName)
        {
            var model = _userService.GetUser(userName);
            var userRole = Roles.GetRolesForUser(userName).FirstOrDefault();

            var userModel = new UserViewModel
            {
                Alias = model.Alias,
                UserName = model.EmailAddress,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                UserType = userRole,
            };

            return View(userModel);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Alias = model.Alias,
                    AvatarUrl = string.Empty,
                    EmailAddress = model.UserName,
                    FirstName = model.FirstName,
                    MiddleName = model.MiddleName,
                    LastName = model.LastName
                    //IsActive = true
                };
                var userRole = Roles.GetRolesForUser(model.UserName).FirstOrDefault();
                if (userRole != model.UserType)
                {
                    Roles.RemoveUserFromRole(model.UserName, userRole);
                    Roles.AddUserToRole(model.UserName, model.UserType);
                }
                _userService.UpdateUser(user);
            }

            if (!WebSecurity.UserExists(model.UserName))
            {
                //WebSecurity.CreateUserAndAccount(model.UserName, "123456", new { FirstName = model.FirstName, LastName = model.LastName, Alias = model.Alias, CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, IsActive = true, IsOnline = false });
                //WebSecurity.CreateUserAndAccount(model.UserName, "123456");                
                Roles.AddUserToRole(model.UserName, model.UserType);

            }

            return RedirectToAction("GetUserList");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                //_userService.MarkOnline(model.UserName);
                return RedirectToAction("Chat");
            }

            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Server");
        }

        public JsonResult GetEmoticons()
        {
            var emoticons = new EmoctionParser().GetAutoReplaceEmoticons();
            var result = Json(new {Emoticons = emoticons, JsonRequestBehavior.AllowGet});
            return result;
        }

        #endregion

        #region Private Methods

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        
        #endregion
    }
}
