using SmartEmpAPI.DTOs;
using SmartEmpAPI.Models.SmartEmpAPI.Models;

namespace SmartEmpAPI.Models
{
    public class Attendance    {
        public Response Resp { get; set; }  // Holds API response details

        public int AttendanceID { get; set; }          // Unique identifier
        public int EmployeeID { get; set; }                // Employee reference
        public DateTime AttendanceDate { get; set; }   // Date of attendance
        public TimeSpan? CheckInTime { get; set; }     // Time of check-in (nullable)
        public TimeSpan? CheckOutTime { get; set; }    // Time of check-out (nullable)
        public string Status { get; set; }             // Present, Absent, Late, etc.
        public decimal? WorkHours { get; set; }        // Total working hours (nullable)
        public bool FaceRecognitionVerified { get; set; } // Face recognition status
        public string CreatedBy { get; set; }          // User who recorded this
        public DateTime CreatedDate { get; set; }      // Record creation date
        public string ModifiedBy { get; set; }         // User who last modified
        public DateTime? ModifiedDate { get; set; }    // Last modified date (nullable)
        public string? CurrentDate { get; internal set; }
        public string? CurrentDay { get; internal set; }
        // User details
        public EmployeeDetails Emp { get; set; }  // Navigation property
    }
    public class AttendanceResponse
    {
        public Response Resp { get; set; }
        public List<Dictionary<string, object>> EmployeeData { get; set; }
    }
}
