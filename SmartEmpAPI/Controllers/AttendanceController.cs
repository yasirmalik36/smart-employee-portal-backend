using Microsoft.AspNetCore.Mvc;
using SmartEmpAPI.Services;
using SmartEmpAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartEmpAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SmartEmpAPI.Models.SmartEmpAPI.Models;
using SmartEmpAPI.Helpers;
namespace SmartEmpAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IFaceRecognition _faceRecongnition;
        private readonly IHttpContextAccessor _httpContext;
        private string _userName = string.Empty;
        private string _userId = string.Empty;
        private string _iP = string.Empty;
        public AttendanceController(IAttendanceService AttendanceService, IFaceRecognition faceRecognition, IHttpContextAccessor httpContext)
        {
            _attendanceService = AttendanceService;
            _faceRecongnition = faceRecognition;
            _httpContext = httpContext;
            //_userName = AESencryption.DecryptData(_encryptionKey, _httpContext.HttpContext.User.Claims.ToList()[1].Value);
            _userName = _httpContext.HttpContext.User.Claims.ToList()[1].Value + " " + _httpContext.HttpContext.User.Claims.ToList()[2].Value;
            _userId = _httpContext.HttpContext.User.Claims.ToList()[0].Value;
            _iP = Helper.GetIp(_httpContext.HttpContext);

        }

        [HttpPost("GetAttendance")]
        public IActionResult GetAttendance([FromBody] AttendanceRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            try
            {
                var response = _attendanceService.GetEmployeeAttendance(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPost("CheckAttendanceStatus")]
        public IActionResult CheckAttendanceStatus([FromBody] AttendanceRequest request)
        {
            if (request == null || request.EmployeeId <= 0)
            {
                return BadRequest("Invalid request data. Please provide a valid Employee ID.");
            }

            try
            {
                var response = _attendanceService.CheckAttendanceStatus(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
        [HttpPost("ManualAttendance")]
        public IActionResult MarkManualAttendance([FromBody] AttendanceRequest request)
        {
            if (string.IsNullOrEmpty(request.AttendanceFlag) || string.IsNullOrEmpty(request.EmployeeId.ToString()))
            {
                return BadRequest("Invalid request data.Please provide Attendance Flag and Employee ID ");
            }
            try
            {
                request.DeviceInfo = "Employee Portal";
                request.Location = "Office";
                request.CreatedBy = _userName;
                request.FaceRecognitionVerified = false;
                request.Biometric = false;

                var response = _attendanceService.MarkEmployeeAttendance(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }

        [HttpPost("GetAllLeaves")]
        public IActionResult GetAllLeaves([FromBody] AttendanceRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            List <Leaves> leaves = _attendanceService.GetAllLeaves(request);
            return Ok(leaves);
        }

        [HttpPost("MarkAttendance")]
        public async Task<IActionResult> MarkAttendance(IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return BadRequest("The ImageFile field is required.");
            }
            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            var result = await _faceRecongnition.ProcessAttendanceAsync( _userName, tempPath);

            System.IO.File.Delete(tempPath);

            return Ok(result);
        }


        }
}
