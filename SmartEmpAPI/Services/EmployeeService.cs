using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SmartEmpAPI.DAL;
using SmartEmpAPI.Interfaces;
using SmartEmpAPI.Models;
using SmartEmpAPI.Models.SmartEmpAPI.Models;
using System.Data;
using SmartEmpAPI.Helpers;

namespace SmartEmpAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DatabaseHelper _databaseHelper;

        public EmployeeService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public EmployeeResponse GetEmployeeInfoByEmployeeID(EmployeeRequest request)
        {

            var parameters = new List<SqlParameter>
               {
                   new SqlParameter("@EmployeeID", request.EmployeeId > 0 ? request.EmployeeId : (object)DBNull.Value),
                   new SqlParameter("@PageNumber", request.PageNumber > 0 ? request.PageNumber : 1),
                   new SqlParameter("@PageSize", request.PageSize > 0 ? request.PageSize : 10),
              
               };

            var (dataSet, response) = _databaseHelper.ExecuteSPWithGenericOutput("PRC_Get_Employees_List", parameters.ToArray());

            var employeeList = Helper.ConvertDataSetToDictionaryList(dataSet);

            return new EmployeeResponse
            {
                Resp = response,
                EmployeeData = employeeList
            };
        }


        public EmployeeResponse GetEmployeeDetails(string employeeIdOrName)
        {
            var parameters = new List<SqlParameter>
          {
              new SqlParameter("@EmployeeID_Name", string.IsNullOrEmpty(employeeIdOrName) ? (object)DBNull.Value : employeeIdOrName)
          };

            // Call the correct method
            var (dataSet, response) = _databaseHelper.ExecuteSPWithGenericOutput("PRC_Get_Employee_Details", parameters.ToArray());
            var employeeList = Helper.ConvertDataSetToDictionaryList(dataSet);

            return new EmployeeResponse
            {
                Resp = response,
                EmployeeData = employeeList
            };
        }

        public PasswordResetResponse ResetEmployeePassword(PasswordResetRequest request)
        {
            var generatedPasswordParam = new SqlParameter("@GeneratedPassword", SqlDbType.NVarChar, 255)
            {
                Direction = ParameterDirection.Output
            };

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeID", request.EmployeeId),
                new SqlParameter("@NewPassword", string.IsNullOrWhiteSpace(request.NewPassword) ? (object)DBNull.Value : request.NewPassword),
                new SqlParameter("@OldPassword", string.IsNullOrWhiteSpace(request.OldPassword) ? (object)DBNull.Value : request.OldPassword),
                generatedPasswordParam

            };
         
            var  response = _databaseHelper.ExecuteSPResponse("PRC_Reset_Employee_Password", parameters.ToArray());

            return new PasswordResetResponse
            {
                Resp = response,
                GeneratedPassword = generatedPasswordParam.Value?.ToString()
            };
        }

    }
}
