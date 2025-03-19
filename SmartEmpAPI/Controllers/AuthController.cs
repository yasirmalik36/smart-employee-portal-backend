using Microsoft.AspNetCore.Mvc;
using SmartEmpAPI.Helpers;
using SmartEmpAPI.Interfaces;
using SmartEmpAPI.Models;
using SmartEmpAPI.Services;

namespace SmartEmpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Login request cannot be null.");
            }

             var loginResponse = _authService.Login(loginRequest);

            if (loginResponse == null)
            {
                return Unauthorized("Invalid email or password, or user is inactive.");
            }

            // If login is successful, return the LoginResponse with user details and token
            return Ok(loginResponse);
        }
    }
}
