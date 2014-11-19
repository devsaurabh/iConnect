using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using iConnect.Data;
using iConnect.Data.Model;
using System.Web.Security;

namespace iConnect.Server.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<ChatContext>(null);

                try
                {
                    using (var context = new ChatContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                            
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("ChatConnection", "Users", "UserId", "EmailAddress", autoCreateTables: true);
                    if (WebSecurity.Initialized)
                    {
                        if (!Roles.RoleExists(UserType.Admin.ToString()))
                            Roles.CreateRole(UserType.Admin.ToString());
                        if (!Roles.RoleExists(UserType.Normal.ToString()))
                            Roles.CreateRole(UserType.Normal.ToString());

                        if (!WebSecurity.UserExists("manpreet.singh@cardinalts.com"))
                        {
                            WebSecurity.CreateUserAndAccount("manpreet.singh@cardinalts.com", "123456", new { FirstName = "Manpreet", LastName = "Singh", Alias = "Mapreet", CreatedOn = DateTime.Now, ModifiedOn = DateTime.Now, IsActive = true, IsOnline = true });                          
                            
                            Roles.AddUserToRole("manpreet.singh@cardinalts.com", UserType.Admin.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
