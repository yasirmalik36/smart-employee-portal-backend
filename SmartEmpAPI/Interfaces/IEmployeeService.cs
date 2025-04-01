using SmartEmpAPI.Models;
using SmartEmpAPI.Models.SmartEmpAPI.Models;

namespace SmartEmpAPI.Interfaces
{
    public interface IEmployeeService
    {
        EmployeeResponse GetEmployeeInfoByEmployeeID(EmployeeRequest request);
        EmployeeResponse GetEmployeeDetails(string employeeIdOrName);
        PasswordResetResponse ResetEmployeePassword(PasswordResetRequest request);
    }
}
