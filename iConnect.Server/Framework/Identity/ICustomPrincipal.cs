using System.Security.Principal;
using System.Web.Security;

namespace iConnect.Server.Framework.Identity
{
    public interface ICustomPrincipal : IPrincipal
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Alias { get; set; }
        string Avatar { get; set; }
    }

    public class CustomPrincipal : ICustomPrincipal
    {

        public CustomPrincipal(string userName)
        {
            this.Identity = new GenericIdentity(userName);
        }

        public bool IsInRole(string role)
        {
            return Identity != null && Identity.IsAuthenticated &&
           !string.IsNullOrWhiteSpace(role) && Roles.IsUserInRole(Identity.Name, role);
        }

        public IIdentity Identity { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }
        public string Avatar { get; set; }
    }

    public class CustomPrincipalSerializedModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Avatar { get; set; }

        public string Alias { get; set; }
    }
}
