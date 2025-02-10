using SmartEmpAPI.Models;

namespace SmartEmpAPI.Interfaces
{
    public interface IAuthService
    {
        LoginResponse Login(string email, string passwordHash);
    }
}
