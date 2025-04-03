using SmartEmpAPI.DTOs;
using SmartEmpAPI.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IO;
using SmartEmpAPI.Interfaces;

namespace SmartEmpAPI.Services
{
   
    public class FaceRecognitionService : IFaceRecognition
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IAttendanceService _attendanceService;

        public FaceRecognitionService(HttpClient httpClient, IConfiguration configuration, IAttendanceService AttendanceService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _attendanceService = AttendanceService;
        }

        public async Task<FaceRecognitionResponse> SaveFaceAsync(string employeeId, string createdBy, string imagePath)
        {
            string apiUrl = $"{_configuration["FaceRecognition:ApiUrl"]}/face-recognition/save-face";

            return await PostFormDataAsync(apiUrl, employeeId, createdBy, imagePath);
        }

        public async Task<Attendance> ProcessAttendanceAsync(string createdBy, string imagePath)
        {
            string faceRecognitionUrl = $"{_configuration["FaceRecognition:ApiUrl"]}/face-recognition/verify-face";
            FaceRecognitionResponse faceRecognitionResult = await PostFormDataAsync(faceRecognitionUrl, imagePath: imagePath);

            if (faceRecognitionResult?.Code == "00" && faceRecognitionResult.Employee_id.HasValue)
            {
                AttendanceRequest request = new AttendanceRequest
                {
                    EmployeeId = faceRecognitionResult.Employee_id.Value,
                    CreatedBy = createdBy,
                    FaceRecognitionVerified = true
                };

                return _attendanceService.MarkEmployeeAttendance(request);
            }
            else
            {
                return new Attendance
                {
                    Resp = new Response
                    {
                        Code = faceRecognitionResult?.Code,
                        Message = faceRecognitionResult?.Message,
                        Description = faceRecognitionResult?.Description
                    }
                };
            }
        }

        public async Task<FaceRecognitionResponse> CheckLivenessAsync(string imagePath)
        {
            string apiUrl = $"{_configuration["FaceRecognition:ApiUrl"]}/face-recognition/check-liveness";

            using var form = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(imagePath);
            using var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(streamContent, "file", Path.GetFileName(imagePath));

            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, form);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<FaceRecognitionResponse>();
            }

            return new FaceRecognitionResponse
            {
                Code = "01",
                Message = "Failure",
                Description = "Liveness check API call failed"
            };
        }

        private async Task<FaceRecognitionResponse> PostFormDataAsync(string apiUrl, string employeeId = null, string createdBy = null, string imagePath = null)
        {
            using var formData = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(employeeId))
            {
                formData.Add(new StringContent(employeeId), "employee_id");
            }

            if (!string.IsNullOrEmpty(createdBy))
            {
                formData.Add(new StringContent(createdBy), "created_by");
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                if (!File.Exists(imagePath))
                {
                    Console.WriteLine("File does not exist.");
                    return null;
                }

                var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                var imageContent = new StreamContent(imageStream);

                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); 

                formData.Add(imageContent, "image_file", Path.GetFileName(imagePath));

                imageStream = null; 
            }

            try
            {
                var response = await _httpClient.PostAsync(apiUrl, formData);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<FaceRecognitionResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

    }
}