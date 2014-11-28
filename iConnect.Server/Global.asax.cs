using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using iConnect.Data;
using System.Data.Entity.Infrastructure;
using iConnect.Server.Framework.Identity;
using WebMatrix.WebData;
using System;
using System.Threading;


namespace iConnect.Server
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serializer = new JavaScriptSerializer();
                if (authTicket.UserData == "OAuth") return;
                var serializeModel = serializer.Deserialize<CustomPrincipalSerializedModel>(authTicket.UserData);
                var newUser = new CustomPrincipal(authTicket.Name)
                {
                    FirstName = serializeModel.FirstName,
                    LastName = serializeModel.LastName,
                    Alias = serializeModel.Alias,
                    Avatar = serializeModel.Avatar
                };
                if (HttpContext.Current != null)
                    HttpContext.Current.User = newUser;
            }
        }

       
    }
}