using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using iConnect.Data.ApplicationServices;
using iConnect.Data.ApplicationServices.Contract;
using iConnect.Server.Framework.Identity;
using iConnect.Server.ViewModels;
using WebMatrix.WebData;

namespace iConnect.Server.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Private Members

        private readonly IUserService _userService;

        //public AccountController()
        //{
        //    _userService = new UserService();
        //}

        #endregion

        #region Actions
       
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                CreateAuthenticationTicket(model.UserName);
                return RedirectToAction("Chat");
            }
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Server");
        }

        #endregion

        #region Private Methods
        
        private void CreateAuthenticationTicket(string username)
        {
            var authUser = _userService.GetUser(username);
            var serializeModel = new CustomPrincipalSerializedModel
            {
                FirstName = authUser.FirstName,
                LastName = authUser.LastName,
                Alias = authUser.Alias,
                Avatar = authUser.AvatarUrl
            };

            var serializer = new JavaScriptSerializer();
            var userData = serializer.Serialize(serializeModel);

            var authTicket = new FormsAuthenticationTicket(1, username, DateTime.Now, DateTime.Now.AddHours(8), false, userData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
        }

        #endregion

    }
}
