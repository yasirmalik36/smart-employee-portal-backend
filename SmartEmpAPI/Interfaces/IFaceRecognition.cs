using SmartEmpAPI.DTOs;
using SmartEmpAPI.Models;

namespace SmartEmpAPI.Interfaces
{
    public interface IFaceRecognition
    {
        Task<FaceRecognitionResponse> SaveFaceAsync(string employeeId, string createdBy, string imagePath);
        Task<Attendance> ProcessAttendanceAsync(string createdBy, string imagePath);
        Task<FaceRecognitionResponse> CheckLivenessAsync(string imagePath);
        EmployeeFaceStatusResponse GetEmployeeFaceStatus(string employeeIdOrName);
    }
}
