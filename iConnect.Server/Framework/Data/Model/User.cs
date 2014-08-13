﻿using System;
using System.Collections.Generic;

namespace iConnect.Server.Framework.Data.Model
{
    public class User : BaseEntity
    {
        /// <summary>
        ///  Unique autogenerated property
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  First name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///  Middle name of the user (optional)
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        ///  Last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///  Email address of the user which will be treated as username
        /// </summary>
        public string EmailAddress { get; set; }    

        /// <summary>
        ///  Password used as login
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///  Alias name for the user
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        ///  Avatar image url
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        ///  Type of user
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        ///  List of messages sent by user
        /// </summary>
        public virtual List<Message> Messages { get; set; }

        /// <summary>
        ///  Status of user user for soft deletion
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///  Online status of the user
        /// </summary>
        public bool IsOnline { get; set; }
    }

    public class Message : BaseEntity
    {
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public User User { get; set; }
        public DateTime SentOn { get; set; }
    }

    public abstract class BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public enum UserType
    {
        Admin,
        Normal
    }

   
}
