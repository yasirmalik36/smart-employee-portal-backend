using SmartEmpAPI.Models;
using System.Collections.Generic;

namespace SmartEmpAPI.Interfaces
{
    public interface IAttendanceService
    {
        List<Attendance> GetAttendanceInfoByUserID(AttendanceRequest request);
        List<Leaves> GetAllLeaves(AttendanceRequest request);
    }
}
