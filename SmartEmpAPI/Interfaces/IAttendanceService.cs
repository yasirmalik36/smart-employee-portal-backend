using SmartEmpAPI.DTOs;
using SmartEmpAPI.Models;
using System.Collections.Generic;

namespace SmartEmpAPI.Interfaces
{
    public interface IAttendanceService
    {
        EmployeeAttendanceResponse GetEmployeeAttendance(AttendanceRequest request);
        List<Leaves> GetAllLeaves(AttendanceRequest request);
        Attendance MarkEmployeeAttendance(AttendanceRequest request);
        AttendanceResponse CheckAttendanceStatus(AttendanceRequest request);

    }
}
