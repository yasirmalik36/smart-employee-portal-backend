using SmartEmpAPI.DAL;
using SmartEmpAPI.Models;
using SmartEmpAPI.Interfaces;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure.Core;
using SmartEmpAPI.DTOs;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Headers;
using SmartEmpAPI.Models.SmartEmpAPI.Models;
using SmartEmpAPI.Helpers;
using Response = SmartEmpAPI.DTOs.Response;

namespace SmartEmpAPI.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AttendanceService(HttpClient httpClient, IConfiguration configuration, DatabaseHelper databaseHelper)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _databaseHelper = databaseHelper;
        }

        public EmployeeAttendanceResponse GetEmployeeAttendance(AttendanceRequest request)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmployeeID", request.EmployeeId == 0 ? (object)DBNull.Value : request.EmployeeId),
                new SqlParameter("@DepartmentID", request.DepartmentId == 0 ? (object)DBNull.Value : request.DepartmentId),
                new SqlParameter("@DesignationID", request.DesignationId == 0 ? (object)DBNull.Value : request.DesignationId),
                new SqlParameter("@Status", request.Status == "" ? (object)DBNull.Value : request.Status),
                new SqlParameter("@ShiftID", request.ShiftId == 0 ? (object)DBNull.Value : request.ShiftId),
                new SqlParameter("@FromDate", request.FromDate.HasValue ? request.FromDate.Value : (object)DBNull.Value),
                new SqlParameter("@ToDate", request.ToDate.HasValue ? request.ToDate.Value : (object)DBNull.Value),
                new SqlParameter("@PageNumber", request.PageNumber),
                new SqlParameter("@PageSize", request.PageSize),
                new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output },
                new SqlParameter("@Code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output },
                new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
            };

            var (dataSet, outputParams) = _databaseHelper.ExecuteStoredProcedurewithOutput("PRC_Get_Employee_Attendance", parameters.ToArray());
            var attendanceList = Helper.ConvertDataSetToDictionaryList(dataSet);

            return new EmployeeAttendanceResponse
            {
                Resp = new Response
                {
                    Code = outputParams["@Code"]?.ToString(),
                    Message = outputParams["@Message"]?.ToString(),
                    Description = outputParams["@Description"]?.ToString(),
                    TotalRecords = outputParams["@TotalRecords"]?.ToString()
                },
                AttendanceData = attendanceList

            };
        }

        public List<Leaves> GetAllLeaves(AttendanceRequest request)
        {
            List<Leaves> leaveList = new List<Leaves>();
            var parameters = new[]
             {
                 new SqlParameter("@UserID", request.EmployeeId),
                 new SqlParameter("@DateFrom", request.FromDate),
                 new SqlParameter("@DateTo", request.ToDate)
             };
            // Execute stored procedure with parameters
            DataSet dataSet = _databaseHelper.ExecuteStoredProcedure("GetAllLeavesByUserID", parameters);

            // Ensure dataset contains tables
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[0]; // Extract the first DataTable

                foreach (DataRow row in dataTable.Rows)
                {
                    leaveList.Add(new Leaves
                    {
                        LeaveID = (int)row["LeaveID"],
                        EmployeeID = (int)row["EmployeeID"],
                        LeaveType = row["LeaveType"].ToString(),
                        StartDate = (DateTime)row["StartDate"],
                        EndDate = (DateTime)row["EndDate"],
                        TotalDays = (int)row["TotalDays"],
                        Reason = row["Reason"].ToString(),
                        Status = row["Status"].ToString(),
                        ApprovedBy = row["ApprovedBy"].ToString(),
                        CreatedBy = row["CreatedBy"].ToString(),
                        CreatedDate = (DateTime)row["CreatedDate"],
                        ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString(),
                        ModifiedDate = row["ModifiedDate"] == DBNull.Value ? null : (DateTime?)row["ModifiedDate"]
                    });
                }
            }

            return leaveList;
        }




        public Attendance MarkEmployeeAttendance(AttendanceRequest request)
        {
            // Define input parameters
            var parameters = new[]
     {
        new SqlParameter("@EmployeeID", SqlDbType.Int) { Value = request.EmployeeId },
        new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 100) { Value = request.CreatedBy },
        new SqlParameter("@FaceRecognitionVerified", SqlDbType.Bit) { Value = request.FaceRecognitionVerified },
        new SqlParameter("@Biometric", SqlDbType.Bit) { Value = request.Biometric },
        new SqlParameter("@AttendanceFlag", SqlDbType.VarChar, 10) { Value = string.IsNullOrEmpty(request.AttendanceFlag) ? (object)DBNull.Value : request.AttendanceFlag },

        new SqlParameter("@DeviceInfo", SqlDbType.NVarChar, 255) { Value = (object)request.DeviceInfo ?? DBNull.Value },
        new SqlParameter("@Location", SqlDbType.NVarChar, 255) { Value = (object)request.Location ?? DBNull.Value },

        // Output parameters
        new SqlParameter("@Message", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output },
        new SqlParameter("@Code", SqlDbType.NVarChar, 2) { Direction = ParameterDirection.Output },
        new SqlParameter("@Description", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output },
        new SqlParameter("@CheckInTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },
        new SqlParameter("@CheckOutTime", SqlDbType.DateTime) { Direction = ParameterDirection.Output },
        new SqlParameter("@CurrentDate", SqlDbType.NVarChar, 25) { Direction = ParameterDirection.Output },
        new SqlParameter("@CurrentDay", SqlDbType.NVarChar, 15) { Direction = ParameterDirection.Output },
        new SqlParameter("@Status", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output },
        new SqlParameter("@WorkHours", SqlDbType.Decimal) { Precision = 5, Scale = 2, Direction = ParameterDirection.Output },

        // Additional output parameters
        new SqlParameter("@FirstName", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output },
        new SqlParameter("@LastName", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output },
        new SqlParameter("@ProfileImage", SqlDbType.VarBinary, -1) { Direction = ParameterDirection.Output },
        new SqlParameter("@Designation", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output }
    };

            // Execute stored procedure
            var (dataSet, outputParams) = _databaseHelper.ExecuteStoredProcedurewithOutput("PRC_Mark_Employee_Attendance", parameters);

            // Create Attendance object
            var attendance = new Attendance
            {
                Resp = new Response
                {
                    Message = outputParams["@Message"]?.ToString(),
                    Code = outputParams["@Code"]?.ToString(),
                    Description = outputParams["@Description"]?.ToString()
                },

                EmployeeID = request.EmployeeId,
                CheckInTime = outputParams["@CheckInTime"] != DBNull.Value ? DateTime.Parse(outputParams["@CheckInTime"].ToString()).TimeOfDay : (TimeSpan?)null,
                CheckOutTime = outputParams["@CheckOutTime"] != DBNull.Value ? DateTime.Parse(outputParams["@CheckOutTime"].ToString()).TimeOfDay : (TimeSpan?)null,
                CurrentDate = outputParams["@CurrentDate"]?.ToString(),
                CurrentDay = outputParams["@CurrentDay"]?.ToString(),
                Status = outputParams["@Status"]?.ToString(),
                WorkHours = outputParams["@WorkHours"] != DBNull.Value ? Convert.ToDecimal(outputParams["@WorkHours"]) : (decimal?)null,
                FaceRecognitionVerified = request.FaceRecognitionVerified,



                // Assign the missing user details
                Emp = new EmployeeDetails
                {
                    FirstName = outputParams["@FirstName"]?.ToString(),
                    LastName = outputParams["@LastName"]?.ToString(),
                    Designation = outputParams["@Designation"]?.ToString(),
                    ProfilePic = outputParams["@ProfileImage"] as byte[]
                }
            };

            return attendance;
        }

        public AttendanceResponse CheckAttendanceStatus(AttendanceRequest request)
        {
            // Validate required fields if needed
            if (request.EmployeeId <= 0)
            {
                throw new ArgumentException("Invalid Employee ID.");
            }

            // Prepare SQL parameters
            var parameters = new List<SqlParameter>
          {
            new SqlParameter("@EmployeeID", request.EmployeeId),
            new SqlParameter("@AttendanceID", (request.AttendanceID.HasValue && request.AttendanceID.Value != 0) ? request.AttendanceID.Value : (object)DBNull.Value),
            new SqlParameter("@FromDate", request.FromDate.HasValue ? request.FromDate.Value : (object)DBNull.Value),
            new SqlParameter("@ToDate", request.ToDate.HasValue ? request.ToDate.Value : (object)DBNull.Value),
           };

            var (dataSet, response) = _databaseHelper.ExecuteSPWithGenericOutput("PRC_Check_Attendance_Status", parameters.ToArray());

            var attendanceList = Helper.ConvertDataSetToDictionaryList(dataSet);

          
            return new AttendanceResponse
            {
                Resp = response,
                EmployeeData = attendanceList
            };
        }

    }
}
