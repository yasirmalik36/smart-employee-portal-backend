namespace SmartEmpAPI.DTOs
{
    public class EmployeeFaceStatusResponse
    {
        public Response Resp { get; set; }
        public List<Dictionary<string, object>> EmployeeFaces { get; set; }
    }
}
