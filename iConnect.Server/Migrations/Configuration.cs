using System;
using iConnect.Data;
using System.Data.Entity.Migrations;
using iConnect.Data.Model;

namespace iConnect.Server.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ChatContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ChatContext context)
        {
            var adminUser = new User
            {
                FirstName = "System",
                LastName = "Admin",
                EmailAddress = "sysadmin@cardinalts.com",
                UserType = UserType.Admin,
                Alias = "SERVER",
                IsActive = true,
                IsOnline = true,
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            var normalUser = new User
            {
                FirstName = "Saurabh",
                LastName = "Singh",
                EmailAddress = "saurabh.singh@cardinalts.com",
                UserType = UserType.Normal,
                Alias = "Saurabh",
                IsActive = true,
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            context.Users.AddOrUpdate(adminUser);
            context.Users.AddOrUpdate(normalUser);
        }
    }
}
