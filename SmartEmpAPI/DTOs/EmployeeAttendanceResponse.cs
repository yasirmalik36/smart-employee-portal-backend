using System.Data;

namespace SmartEmpAPI.DTOs
{
    public class EmployeeAttendanceResponse
    {
        public Response Resp { get; set; }
        public List<Dictionary<string, object>> AttendanceData { get; set; }

    }
    public class EmployeeLeavesResponse
    {
        public Response Resp { get; set; }
        public List<Dictionary<string, object>> LeavesData { get; set; }

    }
}
