using System.Threading.Tasks;

namespace iConnect.Data.ApplicationServices.Contract
{
    public interface IAuthenticationService
    {
        Task<bool> Validate(string userName, string password);
    }
}
