namespace SmartEmpAPI.DTOs
{
    public class Response
    {
        public string Code { get; set; }        
        public string Message { get; set; }       
        public string Description { get; set; }
        public string TotalPages { get; set; } = "0";

    }
}
