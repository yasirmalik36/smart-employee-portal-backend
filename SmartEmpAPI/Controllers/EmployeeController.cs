using Microsoft.AspNetCore.Mvc;
using SmartEmpAPI.Services;
using SmartEmpAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartEmpAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SmartEmpAPI.Models.SmartEmpAPI.Models;

namespace SmartEmpAPI.Controllers
{
   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _EmployeeService;

        public EmployeeController(IEmployeeService EmployeeService)
        {
            _EmployeeService = EmployeeService;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            List<UserModel> users = _EmployeeService.GetUsers();
            return Ok(users);
        }

        [HttpGet("GetEmployeeInfoByUserID/{userID}")]
        public IActionResult GetEmployeeInfoByUserID(int userID)
        {
            EmployeeResponse employee = _EmployeeService.GetEmployeeInfoByUserID(userID);
            return Ok(employee);
        }
    }
}
