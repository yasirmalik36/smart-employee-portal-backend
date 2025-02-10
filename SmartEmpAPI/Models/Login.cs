namespace SmartEmpAPI.Models
{


    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public string IPAddress { get; set; }
    }

    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool C { get; set; }
        public bool R { get; set; }
        public bool U { get; set; }
        public bool D { get; set; }
        public bool E { get; set; }
        public bool Extra { get; set; }
        public int MappingID { get; set; }
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string ActivityURL { get; set; }
    }

    public class LoginResponse
    {
        public User User { get; set; }
        public List<Role> Roles { get; set; }
        public string StatusMessage { get; set; }
        public string StatusCode { get; set; }

    }

}

