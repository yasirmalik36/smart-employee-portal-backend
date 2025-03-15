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
        private readonly IHttpContextAccessor _httpContext;
        private string _userName = string.Empty;
        private string _userId = string.Empty;
        private string _iP = string.Empty;
        public AttendanceController(IAttendanceService AttendanceService, IHttpContextAccessor httpContext)
        {
            _attendanceService = AttendanceService;
            _httpContext = httpContext;
            //_userName = AESencryption.DecryptData(_encryptionKey, _httpContext.HttpContext.User.Claims.ToList()[1].Value);
            _userName = _httpContext.HttpContext.User.Claims.ToList()[1].Value;
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
                // Return a BadRequest with a proper message if the file is not provided
                return BadRequest("The ImageFile field is required.");
            }
            // Save image temporarily
            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            // Call attendance service
            var result = await _attendanceService.ProcessAttendanceAsync( _userName, tempPath);

            // Delete temp image file
            System.IO.File.Delete(tempPath);

            return Ok(result);
        }

        // [AllowAnonymous] // ✅ Makes the API publicly accessible
        [HttpPost("check-liveness")]
        public async Task<IActionResult> CheckLiveness( IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return BadRequest(new
                {
                    Code = "01",
                    Message = "Failure",
                    Reason = "The ImageFile field is required."
                });
            }

            // Save the uploaded image temporarily
            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            // Call the liveness detection service
            var result = await _attendanceService.CheckLivenessAsync(tempPath);

            // Delete the temporary file after processing
            System.IO.File.Delete(tempPath);

            return Ok(result);
        }
    }
}
