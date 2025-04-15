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
using Microsoft.IdentityModel.Tokens;

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
            _userName = _httpContext.HttpContext.User.Claims.ToList()[1].Value +" " + _httpContext.HttpContext.User.Claims.ToList()[2].Value;
            _userId = _httpContext.HttpContext.User.Claims.ToList()[0].Value;
            _iP = Helper.GetIp(_httpContext.HttpContext);
        }


        [HttpGet("GetEmployeeInfoByID")]
        public IActionResult GetEmployeeInfoByID([FromQuery] EmployeeRequest request)
        {
            try
            {
                var response = _employeeService.GetEmployeeInfoByEmployeeID(request);
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

        [HttpGet("GetEmployeeDetails")]
        public IActionResult GetEmployeeDetails([FromQuery] string employeeIdOrName)
        {
            try
            {
                var response = _employeeService.GetEmployeeDetails(employeeIdOrName);
              
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while processing the request.", Error = ex.Message });
            }
        }

        [HttpPost("ResetEmployeePassword")]
        public IActionResult ResetEmployeePassword([FromBody] PasswordResetRequest request)
        {
            try
            {
                if (request == null || request.EmployeeId <= 0 )
                {
                    return BadRequest(new { Message = "Invalid request parameters." });
                }

                var response = _employeeService.ResetEmployeePassword( request);

                if (response == null)
                {
                    return StatusCode(500, new { Message = "Password reset failed. Please try again." });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while resetting the password.", Error = ex.Message });
            }
        }
        [HttpPost("AddUpdateEmployee")]
        public IActionResult AddUpdateEmployee([FromBody] EmployeeAddUpdateRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { Message = "Invalid request parameters." });
                }

                request.CreatedBy = _userName;
                if(request.EmployeeID != 0)
                {
                    request.ModifiedBy = _userName;
                }
                          
               var response = _employeeService.AddUpdateEmployee(request);

                if (response == null)
                {
                    return StatusCode(500, new { Message = "Failed to add or update employee. Please try again." });
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
