namespace SmartEmpAPI.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }          // Unique identifier
        public int UserID { get; set; }                // Employee reference
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
    }

}
