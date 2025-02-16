using SmartEmpAPI.DAL;
using SmartEmpAPI.Models;
using SmartEmpAPI.Interfaces;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure.Core;

namespace SmartEmpAPI.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly DatabaseHelper _databaseHelper;

        public AttendanceService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Attendance> GetAttendanceInfoByUserID(AttendanceRequest request)
        {
            List<Attendance> attendanceList = new List<Attendance>();

            var parameters = new[]
             {
                 new SqlParameter("@UserID", request.UserID),
                 new SqlParameter("@DateFrom", request.DateFrom),
                 new SqlParameter("@DateTo", request.DateTo)
             };
            // Execute stored procedure with parameters
            DataSet dataSet = _databaseHelper.ExecuteStoredProcedure("GetAttendanceByUserID", parameters);

            // Ensure dataset contains tables and at least one row
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return null;
            // Ensure the dataset has tables and rows
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable = dataSet.Tables[0]; // Extract the first DataTable

                foreach (DataRow row in dataTable.Rows)
                {
                    attendanceList.Add(new Attendance
                    {
                        AttendanceID = (int)row["AttendanceID"],
                        UserID = (int)row["UserID"],
                        AttendanceDate = (DateTime)row["AttendanceDate"],
                        CheckInTime = row["CheckInTime"] == DBNull.Value ? null : ((DateTime)row["CheckInTime"]).TimeOfDay,
                        CheckOutTime = row["CheckOutTime"] == DBNull.Value ? null : ((DateTime)row["CheckOutTime"]).TimeOfDay,
                        Status = row["Status"].ToString(),
                        WorkHours = row["WorkHours"] == DBNull.Value ? null : (decimal?)row["WorkHours"],
                        FaceRecognitionVerified = (bool)row["FaceRecognitionVerified"],
                        CreatedBy = row["CreatedBy"].ToString(),
                        CreatedDate = (DateTime)row["CreatedDate"],
                        ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : row["ModifiedBy"].ToString(),
                        ModifiedDate = row["ModifiedDate"] == DBNull.Value ? null : (DateTime?)row["ModifiedDate"]
                    });
                }
            }

            return attendanceList;
        }

        public List<Leaves> GetAllLeaves(AttendanceRequest request)
        {
            List<Leaves> leaveList = new List<Leaves>();
            var parameters = new[]
             {
                 new SqlParameter("@UserID", request.UserID),
                 new SqlParameter("@DateFrom", request.DateFrom),
                 new SqlParameter("@DateTo", request.DateTo)
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
                        UserID = (int)row["UserID"],
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

    }
}
