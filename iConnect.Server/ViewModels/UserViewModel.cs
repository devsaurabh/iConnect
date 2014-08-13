﻿using System;
using System.ComponentModel.DataAnnotations;

namespace iConnect.Server.ViewModels
{
    public class UserViewModel
    {
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
    }
}