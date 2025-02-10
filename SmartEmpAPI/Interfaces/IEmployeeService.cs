using SmartEmpAPI.Models;
using SmartEmpAPI.Models.SmartEmpAPI.Models;

namespace SmartEmpAPI.Interfaces
{
    public interface IEmployeeService
    {
        List<UserModel> GetUsers();
        EmployeeResponse GetEmployeeInfoByUserID(int userID);
    }
}
