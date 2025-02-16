using Microsoft.AspNetCore.Mvc;
using SmartEmpAPI.Services;
using SmartEmpAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartEmpAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SmartEmpAPI.Models.SmartEmpAPI.Models;
using Azure.Core;

namespace SmartEmpAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService AttendanceService)
        {
            _attendanceService = AttendanceService;
        }

        [HttpPost("GetAttendance")]
        public IActionResult GetAttendance([FromBody] AttendanceRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            List<Attendance> attendance = _attendanceService.GetAttendanceInfoByUserID(request);
            return Ok(attendance);
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

    }
}
