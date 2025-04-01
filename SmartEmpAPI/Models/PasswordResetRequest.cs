namespace SmartEmpAPI.Models
{
    public class PasswordResetRequest
    {
        public int EmployeeId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
