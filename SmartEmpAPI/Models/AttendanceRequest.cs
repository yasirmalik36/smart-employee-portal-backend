namespace SmartEmpAPI.Models
{
    public class AttendanceRequest
    {
        public int UserID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
