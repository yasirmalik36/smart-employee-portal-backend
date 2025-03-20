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
                new SqlParameter("@PageNumber", request.PageNumber),
                new SqlParameter("@PageSize", request.PageSize),
                new SqlParameter("@TotalPages", SqlDbType.Int) { Direction = ParameterDirection.Output },
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
                    TotalPages = outputParams["@TotalPages"]?.ToString()
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



        public async Task<Attendance> ProcessAttendanceAsync(string createdBy, string imagePath)
        {
            // Get the Face Recognition API URL from app settings

            string faceRecognitionUrl = $"{_configuration["FaceRecognition:ApiUrl"]}/face-recognition/verify-face";
            FaceRecognitionResponse faceRecognitionResult = new FaceRecognitionResponse();

            // Call the VerifyFaceAsync method to get the result
            faceRecognitionResult = await VerifyFaceAsync(faceRecognitionUrl, imagePath);

            // Check if the face is matched
            if (faceRecognitionResult.Code == "00" && faceRecognitionResult.Employee_id.HasValue)
            {
                AttendanceRequest request = new AttendanceRequest();
                request.EmployeeId = faceRecognitionResult.Employee_id.Value;
                request.CreatedBy = createdBy;
                request.FaceRecognitionVerified = true;
                return MarkEmployeeAttendance(request);
            }
            else
            {
                return new Attendance
                {
                    Resp = new Response
                    {
                        Code = faceRecognitionResult.Code,
                        Message = faceRecognitionResult.Message,
                        Description = faceRecognitionResult.Description
                    }
                };
            }
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

        private async Task<FaceRecognitionResponse> VerifyFaceAsync(string apiUrl, string imagePath)
        {
            using var formData = new MultipartFormDataContent();
            using var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            using var imageContent = new StreamContent(imageStream);

            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            formData.Add(imageContent, "image_file", Path.GetFileName(imagePath));

            var response = await _httpClient.PostAsync(apiUrl, formData);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<FaceRecognitionResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<FaceRecognitionResponse> CheckLivenessAsync(string imagePath)
        {
            string apiUrl = $"{_configuration["FaceRecognition:ApiUrl"]}/face-recognition/check-liveness";

            using var form = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(imagePath);
            using var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            form.Add(streamContent, "file", Path.GetFileName(imagePath));

            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, form);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FaceRecognitionResponse>();
            }

            return new FaceRecognitionResponse
            {
                Code = "01",
                Message = "Failure",
                Description = "Liveness check API call failed"
            };

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
