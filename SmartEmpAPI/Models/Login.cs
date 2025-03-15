
using SmartEmpAPI.DTOs;
using System.ComponentModel.DataAnnotations;

namespace SmartEmpAPI.Models
{


    public class User
    {
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; }

        [MaxLength(255)]
        public string ProfileImage { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? LastLogin { get; set; }

        [MaxLength(45)]
        public string IPAddress { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public int FailedLoginAttempts { get; set; }

        public DateTime? LockoutEnd { get; set; }
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
        public Response Resp { get; set; }
        public string Token { get; set; }
        public List<Role> Roles { get; set; }
        public bool IsOtpRequired { get; set; } // Indicates if OTP verification is needed
        public bool IsPasswordResetRequired { get; set; } // Force user to reset password
        public bool IsAccountLocked { get; set; } // If the account is locked
        public bool IsFirstLogin { get; set; } // If this is the user's first login
        public User UserProfile { get; set; } 

    }

}

