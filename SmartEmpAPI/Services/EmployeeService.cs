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
using SmartEmpAPI.DTOs;

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

        public Response AddUpdateEmployee(EmployeeAddUpdateRequest request)
        {
            byte[] profilePicBytes = null;

            if (!string.IsNullOrEmpty(request.ProfilePic))
            {
                // Remove the base64 prefix if it exists (e.g., "data:image/png;base64,")
                string base64Data = request.ProfilePic.Contains("base64,")
                    ? request.ProfilePic.Substring(request.ProfilePic.IndexOf("base64,") + 7)
                    : request.ProfilePic;

                // Convert the Base64 string to a byte array
                profilePicBytes = Convert.FromBase64String(base64Data);
            }
            var parameters = new List<SqlParameter>
    {
      new SqlParameter("@EmployeeID", request.EmployeeID == null || request.EmployeeID == 0 ? (object)DBNull.Value : request.EmployeeID),
        new SqlParameter("@FirstName", request.FirstName),
        new SqlParameter("@LastName", request.LastName),
        new SqlParameter("@DesignationID", request.DesignationID),
        new SqlParameter("@DepartmentID", request.DepartmentID),
        new SqlParameter("@Email", request.Email),
        new SqlParameter("@Personal_Email", request.PersonalEmail),
        new SqlParameter("@CNIC", request.CNIC),
        new SqlParameter("@Phone", request.Phone),
        new SqlParameter("@Gender", request.Gender),
        new SqlParameter("@DateOfBirth", request.DateOfBirth ?? (object)DBNull.Value),
        new SqlParameter("@Address", request.Address),
        new SqlParameter("@City", request.City),
        new SqlParameter("@State", request.State),
        new SqlParameter("@ZipCode", request.ZipCode),
        new SqlParameter("@Country", request.Country),
        new SqlParameter("@MaritalStatus", request.MaritalStatus),
        new SqlParameter("@JoiningDate", request.JoiningDate ?? (object)DBNull.Value),
        new SqlParameter("@ShiftID", request.ShiftID ?? (object)DBNull.Value),
        new SqlParameter("@TeamID", request.TeamID ?? (object)DBNull.Value),
        new SqlParameter("@ProfileID", request.ProfileID),
        new SqlParameter("@Highest_degree", request.HighestDegree),
        new SqlParameter("@Institution", request.Institution),
        new SqlParameter("@Year_of_graduation", request.YearOfGraduation ?? (object)DBNull.Value),
        new SqlParameter("@Major", request.Major),
        new SqlParameter("@IsOnProbation", request.IsOnProbation),
        new SqlParameter("@ProbationEndDate", request.ProbationEndDate ?? (object)DBNull.Value),
        new SqlParameter("@EmploymentType", request.EmploymentType),
        new SqlParameter("@WorkLocation", request.WorkLocation),
        new SqlParameter("@ReportingManagerID", request.ReportingManagerID ?? (object)DBNull.Value),
        new SqlParameter("@BloodGroup", request.BloodGroup),
        new SqlParameter("@Emergency_contact_name", request.EmergencyContactName),
        new SqlParameter("@Emergency_contact_number", request.EmergencyContactNumber),
        new SqlParameter("@Emergency_contact_relationship", request.EmergencyContactRelationship),
        new SqlParameter("@Health_condition", request.HealthCondition),
        new SqlParameter("@Disability_status", request.DisabilityStatus),
        new SqlParameter("@Medications", request.Medications),
        new SqlParameter("@Number_of_dependents", request.NumberOfDependents?.ToString() ?? "0"),
        new SqlParameter("@ProfilePic", profilePicBytes ?? (object)DBNull.Value),
        new SqlParameter("@IsActive", request.IsActive),
        new SqlParameter("@CreatedBy", request.CreatedBy),
        new SqlParameter("@ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value),
        new SqlParameter("@Password", "Abc@123")
    };



            var response = _databaseHelper.ExecuteSPResponse("PRC_Add_Update_Employee", parameters.ToArray());
            return response;
        }

    }
}
