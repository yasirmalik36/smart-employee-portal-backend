using Microsoft.AspNetCore.Mvc;
using SmartEmpAPI.Services;
using SmartEmpAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartEmpAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SmartEmpAPI.Helpers;
using Microsoft.AspNetCore.Http;
using System.Linq;
using SmartEmpAPI.Models.SmartEmpAPI.Models;

namespace SmartEmpAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IHttpContextAccessor _httpContext;
        private string _userName = string.Empty;
        private string _userId = string.Empty;
        private string _iP = string.Empty;

        public EmployeeController(IEmployeeService employeeService, IHttpContextAccessor httpContext)
        {
            _employeeService = employeeService;
            _httpContext = httpContext;

            // Extract user details from claims
            _userName = _httpContext.HttpContext.User.Claims.ToList()[1].Value;
            _userId = _httpContext.HttpContext.User.Claims.ToList()[0].Value;
            _iP = Helper.GetIp(_httpContext.HttpContext);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                List<UserModel> users = _employeeService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching users.", Error = ex.Message });
            }
        }

        [HttpGet("GetEmployeeInfoByUserID/{userID}")]
        public IActionResult GetEmployeeInfoByUserID(int userID)
        {
            try
            {
                EmployeeResponse employee = _employeeService.GetEmployeeInfoByUserID(userID);
                if (employee == null)
                {
                    return NotFound(new { Message = "Employee not found." });
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching employee info.", Error = ex.Message });
            }
        }

        [HttpGet("GetEmployeeDetails")]
        public IActionResult GetEmployeeDetails([FromQuery] string employeeIdOrName)
        {
            try
            {
                var response = _employeeService.GetEmployeeDetails(employeeIdOrName);
                if (response == null || response.EmployeeData == null || !response.EmployeeData.Any())
                {
                    return NotFound(new { Message = "No employee details found." });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
        }
    }
}
