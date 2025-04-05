using Microsoft.AspNetCore.Mvc;
using SmartEmpAPI.Services;
using SmartEmpAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartEmpAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SmartEmpAPI.Models.SmartEmpAPI.Models;
using SmartEmpAPI.Helpers;
using Microsoft.AspNetCore.Http;
using System.IO;
using SmartEmpAPI.DTOs;

namespace SmartEmpAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FaceRecognitionController : ControllerBase
    {
        private readonly IFaceRecognition _faceRecognition;
        private readonly IHttpContextAccessor _httpContext;
        private string _userName = string.Empty;

        public FaceRecognitionController(IFaceRecognition faceRecognition, IHttpContextAccessor httpContext)
        {
            _faceRecognition = faceRecognition;
            _httpContext = httpContext;
            _userName = _httpContext.HttpContext.User.Claims.ToList()[1].Value + " " + _httpContext.HttpContext.User.Claims.ToList()[2].Value;
        }

        [HttpPost("SaveFace")]
        public async Task<IActionResult> SaveFace(IFormFile ImageFile, string employeeId)
            
        {
            string createdBy = _userName;
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return BadRequest(new Response
                {
                    Code = "01",
                    Message = "Failure",
                    Description = "The ImageFile field is required."
                });
            }

            if (string.IsNullOrEmpty(employeeId))
            {
                return BadRequest(new Response { Code = "01", Message = "Failure", Description = "Employee ID is required." });
            }

            if (string.IsNullOrEmpty(createdBy))
            {
                return BadRequest(new Response { Code = "01", Message = "Failure", Description = "Created by is required." });
            }

            var tempPath = Path.GetTempFileName();
            try
            {
                using (var stream = new FileStream(tempPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                var result = await _faceRecognition.SaveFaceAsync(employeeId, createdBy, tempPath);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response
                {
                    Code = "01",
                    Message = "Failure",
                    Description = $"An error occurred: {ex.Message}"
                });
            }
            finally
            {
                System.IO.File.Delete(tempPath);
            }
        }
        [HttpPost("CheckLiveness")]
        public async Task<IActionResult> CheckLiveness(IFormFile ImageFile)
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

            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            try
            {
                var result = await _faceRecognition.CheckLivenessAsync(tempPath);
                System.IO.File.Delete(tempPath);
                return Ok(result);
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(tempPath);
                return StatusCode(500, new { Message = "An error occurred during liveness check.", Error = ex.Message });
            }
        }
        [HttpGet("GetEmployeeFaceDetails")]
        public IActionResult GetEmployeeFaceDetails([FromQuery] string employeeIdOrName)
        {
            try
            {
                var response = _faceRecognition.GetEmployeeFaceStatus(employeeIdOrName);
                if (response == null || response.EmployeeFaces == null || !response.EmployeeFaces.Any())
                {
                    return NotFound(new { Message = "No employee face status found." });
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