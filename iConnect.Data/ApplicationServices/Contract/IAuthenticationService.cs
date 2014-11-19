using System.Threading.Tasks;

namespace iConnect.Data.ApplicationServices.Contract
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateAsync(string userName, string password);
        bool Validate(string userName, string password);
    }
}
