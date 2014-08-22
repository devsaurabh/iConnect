namespace iConnect.Data.ApplicationServices.Contract
{
    public interface IAuthenticationService
    {
        bool Validate(string userName, string password);
    }
}
