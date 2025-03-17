using System.ComponentModel.DataAnnotations;

namespace SmartEmpAPI.Models
{
    public class AttendanceRequest
    {
        public int EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string Status { get; set; }
        public int? ShiftId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string CreatedBy { get; set; }
        public bool FaceRecognitionVerified { get; set; } = false;  // Default: 0 (false)
        public bool Biometric { get; set; } = false;  // Default: 0 (false)
        public string AttendanceFlag { get; set; } = "";  // Default: Empty string
        public string DeviceInfo { get; set; }
        public string Location { get; set; }
    }
}
