namespace SmartEmpAPI.Models
{
    public class EmployeeRequest
    {
        public int EmployeeId { get; set; } = 0;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
