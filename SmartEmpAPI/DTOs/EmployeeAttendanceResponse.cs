using System.Data;

namespace SmartEmpAPI.DTOs
{
    public class EmployeeAttendanceResponse
    {
        public Response Resp { get; set; }
        public List<Dictionary<string, object>> AttendanceData { get; set; }

    }
}
