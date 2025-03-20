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

        public List<UserModel> GetUsers()
        {
           

            DataSet dataSet = _databaseHelper.ExecuteStoredProcedurewithoutParam("GetAllUsers");

            List<UserModel> users = new List<UserModel>();
            if (dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    users.Add(new UserModel
                    {
                        User_ID = Convert.ToInt32(row["User_ID"]),
                        USER_NAME = row["USER_NAME"].ToString(),
                        First_Name = row["First_Name"].ToString(),
                        Last_Name = row["Last_Name"].ToString(),
                        Email = row["Email"].ToString(),
                        Role = row["Role"].ToString()
                    });
                }
            }

            return users;
        }

        //public EmployeeResponse GetEmployeeInfoByUserID(int userID)
        //{
        //    EmployeeResponse employeeModel = new EmployeeResponse();

        //    try
        //    {
        //        var parameters = new[]
        //        {
        //    new SqlParameter("@userID", userID),
        //};

        //        // Execute stored procedure
        //        DataSet dataSet = _databaseHelper.ExecuteStoredProcedure("GetEmployeeInfoByUserID", parameters);

        //        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        //        {
        //            DataRow row = dataSet.Tables[0].Rows[0];

        //            employeeModel.StatusMessage = "Success";
        //            employeeModel.StatusCode = "00";

        //            // Fill employee details
        //            employeeModel.EmployeeDetails = new EmployeeDetails
        //            {
        //                EmployeeId = Convert.ToInt32(row["employee_id"]),
        //                UserId = Convert.ToInt32(row["userid"]),
        //                FirstName = row["first_name"].ToString(),
        //                LastName = row["last_name"].ToString(),
        //                Gender = row["gender"].ToString(),
        //                DateOfBirth = Convert.ToDateTime(row["date_of_birth"]),
        //                Email = row["email"].ToString(),
        //                PhoneNumber = row["phone_number"].ToString(),
        //                Address = row["address"].ToString(),
        //                City = row["city"].ToString(),
        //                State = row["state"].ToString(),
        //                PostalCode = row["postal_code"].ToString(),
        //                Country = row["country"].ToString(),
        //                HighestDegree = row["highest_degree"].ToString(),
        //                Institution = row["institution"].ToString(),
        //                YearOfGraduation = Convert.ToInt32(row["year_of_graduation"]),
        //                Major = row["major"].ToString(),
        //                AdditionalCertifications = row["additional_certifications"].ToString(),
        //                LanguagesSpoken = row["languages_spoken"].ToString(),
        //                PreviousEmployer = row["previous_employer"].ToString(),
        //                PreviousJobTitle = row["previous_job_title"].ToString(),
        //                HireDate = Convert.ToDateTime(row["hire_date"]),
        //                ProbationPeriod = row["probation_period"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["probation_period"]),
        //                EmploymentStatus = row["employment_status"].ToString(),
        //                JobTitle = row["job_title"].ToString(),
        //                Department = row["department"].ToString(),
        //                TeamName = row["team_name"].ToString(),
        //                ManagerId = row["manager_id"] == DBNull.Value ? null : (int?)Convert.ToInt32(row["manager_id"]),
        //                Salary = Convert.ToDecimal(row["salary"]),
        //                EmploymentType = row["employment_type"].ToString(),
        //                WorkLocation = row["work_location"].ToString(),
        //                WorkingHoursPerWeek = Convert.ToInt32(row["working_hours_per_week"]),
        //                ContractStartDate = row["contract_start_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["contract_start_date"]),
        //                ContractEndDate = row["contract_end_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["contract_end_date"]),
        //                BankAccountNumber = row["bank_account_number"].ToString(),
        //                BankName = row["bank_name"].ToString(),
        //                NationalId = row["national_id"].ToString(),
        //                PassportNumber = row["passport_number"].ToString(),
        //                FinanceNotes = row["finance_notes"].ToString(),
        //                TaxIdentificationNumber = row["tax_identification_number"].ToString(),
        //                SalaryAccountingCode = row["salary_accounting_code"].ToString(),
        //                RefereeName = row["referee_name"].ToString(),
        //                RefereeContact = row["referee_contact"].ToString(),
        //                RefereeRelationship = row["referee_relationship"].ToString(),
        //                EmergencyContactName = row["emergency_contact_name"].ToString(),
        //                EmergencyContactNumber = row["emergency_contact_number"].ToString(),
        //                EmergencyContactRelationship = row["emergency_contact_relationship"].ToString(),
        //                HealthCondition = row["health_condition"].ToString(),
        //                DisabilityStatus = row["disability_status"].ToString(),
        //                Medications = row["medications"].ToString(),
        //                CreatedDate = Convert.ToDateTime(row["created_date"]),
        //                UpdatedDate = Convert.ToDateTime(row["updated_date"]),
        //                CreatedBy = row["created_by"].ToString(),
        //                ModifiedBy = row["modified_by"].ToString(),
        //                TerminationDate = row["termination_date"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["termination_date"]),
        //                ExitReason = row["exit_reason"].ToString(),
        //                RehireStatus = row["rehire_status"].ToString(),
        //                WorkEmail = row["work_email"].ToString(),
        //                PersonalEmail = row["personal_email"].ToString(),
        //                MaritalStatus = row["marital_status"].ToString(),
        //                NumberOfDependents = Convert.ToInt32(row["number_of_dependents"]),
        //                SocialSecurityNumber = row["social_security_number"].ToString(),
        //                IsActive = Convert.ToBoolean(row["is_active"])
        //            };
        //        }
        //        else
        //        {
        //            employeeModel.StatusMessage = "No data found";
        //            employeeModel.StatusCode = "01";
        //            employeeModel.EmployeeDetails = null;
        //        }
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        employeeModel.StatusMessage = $"SQL Error: {sqlEx.Message}";
        //        employeeModel.StatusCode = "02";
        //        employeeModel.EmployeeDetails = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        employeeModel.StatusMessage = $"Error: {ex.Message}";
        //        employeeModel.StatusCode = "99";
        //        employeeModel.EmployeeDetails = null;
        //    }

        //    return employeeModel;
        //}
        public EmployeeResponse GetEmployeeInfoByUserID(int userID)
        {
            EmployeeResponse employeeModel = new EmployeeResponse();

            try
            {
                var parameters = new[]
                {
                         new SqlParameter("@userID", userID),
                };

                // Execute stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedure("GetEmployeeInfoByUserID", parameters);

                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables[0].Rows[0];

                    employeeModel.StatusMessage = "Success";
                    employeeModel.StatusCode = "00";

                    // Fill employee details
                    employeeModel.EmployeeDetails = new EmployeeDetails
                    {
                        EmployeeId = Convert.ToInt32(row["employee_id"]),
                        UserId = Convert.ToInt32(row["userid"]),
                        FirstName = row["first_name"].ToString(),
                        LastName = row["last_name"].ToString(),
                        Gender = row["gender"].ToString(),
                        DateOfBirth = row["date_of_birth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["date_of_birth"]),
                        Email = row["email"].ToString(),
                        PhoneNumber = row["phone_number"].ToString(),
                        Address = row["address"].ToString(),
                        City = row["city"].ToString(),
                        State = row["state"].ToString(),
                        PostalCode = row["postal_code"].ToString(),
                        Country = row["country"].ToString(),
                        HighestDegree = row["highest_degree"].ToString(),
                        Institution = row["institution"].ToString(),
                        YearOfGraduation = row["year_of_graduation"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["year_of_graduation"]),
                        Major = row["major"].ToString(),
                        AdditionalCertifications = row["additional_certifications"].ToString(),
                        LanguagesSpoken = row["languages_spoken"].ToString(),
                        PreviousEmployer = row["previous_employer"].ToString(),
                        PreviousJobTitle = row["previous_job_title"].ToString(),
                        HireDate = row["hire_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["hire_date"]),
                        ProbationPeriod = row["probation_period"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["probation_period"]),
                        EmploymentStatus = row["employment_status"].ToString(),
                        JobTitle = row["job_title"].ToString(),
                        Department = row["department"].ToString(),
                        TeamName = row["team_name"].ToString(),
                        ManagerId = row["manager_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["manager_id"]),
                        Salary = Convert.ToDecimal(row["salary"]),
                        EmploymentType = row["employment_type"].ToString(),
                        WorkLocation = row["work_location"].ToString(),
                        WorkingHoursPerWeek = Convert.ToInt32(row["working_hours_per_week"]),
                        ContractStartDate = row["contract_start_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["contract_start_date"]),
                        ContractEndDate = row["contract_end_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["contract_end_date"]),
                        BankAccountNumber = row["bank_account_number"].ToString(),
                        BankName = row["bank_name"].ToString(),
                        NationalId = row["national_id"].ToString(),
                        PassportNumber = row["passport_number"].ToString(),
                        FinanceNotes = row["finance_notes"].ToString(),
                        TaxIdentificationNumber = row["tax_identification_number"].ToString(),
                        SalaryAccountingCode = row["salary_accounting_code"].ToString(),
                        RefereeName = row["referee_name"].ToString(),
                        RefereeContact = row["referee_contact"].ToString(),
                        RefereeRelationship = row["referee_relationship"].ToString(),
                        EmergencyContactName = row["emergency_contact_name"].ToString(),
                        EmergencyContactNumber = row["emergency_contact_number"].ToString(),
                        EmergencyContactRelationship = row["emergency_contact_relationship"].ToString(),
                        HealthCondition = row["health_condition"].ToString(),
                        DisabilityStatus = row["disability_status"].ToString(),
                        Medications = row["medications"].ToString(),
                        CreatedDate = row["created_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["created_date"]),
                        UpdatedDate = row["updated_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["updated_date"]),
                        CreatedBy = row["created_by"].ToString(),
                        ModifiedBy = row["modified_by"].ToString(),
                        TerminationDate = row["termination_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["termination_date"]),
                        ExitReason = row["exit_reason"].ToString(),
                        RehireStatus = row["rehire_status"].ToString(),
                        WorkEmail = row["work_email"].ToString(),
                        PersonalEmail = row["personal_email"].ToString(),
                        MaritalStatus = row["marital_status"].ToString(),
                        NumberOfDependents = Convert.ToInt32(row["number_of_dependents"]),
                        SocialSecurityNumber = row["social_security_number"].ToString(),
                        IsActive = Convert.ToBoolean(row["is_active"])
                    };
                }
                else
                {
                    employeeModel.StatusMessage = "No data found";
                    employeeModel.StatusCode = "01";
                    employeeModel.EmployeeDetails = null;
                }
            }
            catch (SqlException sqlEx)
            {
                employeeModel.StatusMessage = $"SQL Error: {sqlEx.Message}";
                employeeModel.StatusCode = "02";
                employeeModel.EmployeeDetails = null;
            }
            catch (Exception ex)
            {
                employeeModel.StatusMessage = $"Error: {ex.Message}";
                employeeModel.StatusCode = "99";
                employeeModel.EmployeeDetails = null;
            }

            return employeeModel;
        }


        public EmployeeDetailsResponse GetEmployeeDetails(string employeeIdOrName)
        {
            var parameters = new List<SqlParameter>
          {
              new SqlParameter("@EmployeeID_Name", string.IsNullOrEmpty(employeeIdOrName) ? (object)DBNull.Value : employeeIdOrName)
          };

            // Call the correct method
            var (dataSet, response) = _databaseHelper.ExecuteSPWithGenericOutput("PRC_Get_Employee_Details", parameters.ToArray());
            var employeeList = Helper.ConvertDataSetToDictionaryList(dataSet);

            return new EmployeeDetailsResponse
            {
                Resp = response,
                EmployeeData = employeeList
            };
        }

    }
}
