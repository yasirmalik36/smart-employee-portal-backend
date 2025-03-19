using SmartEmpAPI.Models;

namespace SmartEmpAPI.Interfaces
{
    public interface IAuthService
    {
        LoginResponse Login(LoginRequest request);
    }
}
