// Controllers/CommonController.cs
using Microsoft.AspNetCore.Mvc;
using SmartEmpAPI.Interfaces;
using SmartEmpAPI.Models;
using SmartEmpAPI.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SmartEmpAPI.Helpers;
using Microsoft.AspNetCore.Http;

namespace SmartEmpAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;
        private readonly IHttpContextAccessor _httpContext;
        private string _userName = string.Empty;
        private string _userId = string.Empty;
        private string _iP = string.Empty;
        public CommonController(ICommonService commonService, IHttpContextAccessor httpContext)
        {
            _commonService = commonService;
            _httpContext = httpContext;
            _userName = _httpContext.HttpContext.User.Claims.ToList()[1].Value + " " + _httpContext.HttpContext.User.Claims.ToList()[2].Value;
            _userId = _httpContext.HttpContext.User.Claims.ToList()[0].Value;
            _iP = Helper.GetIp(_httpContext.HttpContext);

        }

        [HttpPost("GetDropdownData")]
        public async Task<IActionResult> GetDropdownData([FromBody] DropdownRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Param))
            {
                return BadRequest("Invalid request. TableName is required.");
            }

            try
            {
                var (response, data) = await _commonService.GetDropdownDataAsync(request.Param);
                if (response.Code == "00") 
                {
                    return Ok(new { Resp = response, Data = data });
                }
                else
                {
                    return BadRequest(new { Resp = response });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }
    }
}