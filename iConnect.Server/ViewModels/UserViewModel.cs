using iConnect.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;

namespace iConnect.Server.ViewModels
{
    public class UserViewModel
    {

        public UserViewModel()
        {          
            var users = Roles.GetAllRoles().Select(t=> new { Text = t, Value = t});
            UserTypes = new SelectList(users, "Value", "Text");
        }
        public int UserId { get; set; }

        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "first name is required")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Alias is required")]
        public string Alias { get; set; }
        public DateTime RegisteredOn { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public string UserType { get; set; }

        public SelectList UserTypes { get; set; }
    }
}