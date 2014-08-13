using System.Linq;
using System.Web.Mvc;
using iConnect.Server.Framework.ApplicationServices;
using iConnect.Server.Framework.ApplicationServices.Contract;
using iConnect.Server.Framework.Data;
using iConnect.Server.Framework.Data.Model;
using iConnect.Server.ViewModels;

namespace iConnect.Server.Controllers
{
    public class ServerController : Controller
    {
        private readonly IUserService _userService;

        public ServerController()
        {
            var chatContext = new ChatContext();
            _userService = new UserService(chatContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserList()
        {
            var model = _userService.GetAllUsers().Select(usr => new UserViewModel
            {
                UserId = usr.UserId,
                UserName = usr.EmailAddress,
                FirstName = usr.FirstName,
                MiddleName = usr.MiddleName,
                LastName = usr.LastName,
                Alias = usr.Alias,
                IsOnline = usr.IsOnline,
                RegisteredOn = usr.CreatedOn
            }).OrderBy(u=>u.RegisteredOn).ToList();
            return View(model);
        }

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
                Password = "123456",
                AvatarUrl = string.Empty,
                EmailAddress = model.UserName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                UserType = UserType.Normal
            };
            _userService.CreateUser(user);
            return RedirectToAction("GetUserList");
        }

    }
}
