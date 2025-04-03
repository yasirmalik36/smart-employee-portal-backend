
using SmartEmpAPI.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SmartEmpAPI.Models
{


    public class UserLoginInfo
    {
        public int EmployeeID { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DesignationName { get; set; }

        public string Email { get; set; }
        public int ProfileID { get; set; }
        public string Password { get; set; }


        public byte[]? ProfilePic { get; set; }

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
        public bool PasswordResetRequired { get; set; }
}

    public class Profile
    {
        public int ProfileID { get; set; }
        public string ProfileName { get; set; }
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
        public List<Profile> Act { get; set; }
        public bool IsOtpRequired { get; set; } // Indicates if OTP verification is needed
        public bool IsPasswordResetRequired { get; set; } // Force user to reset password
        public bool IsAccountLocked { get; set; } // If the account is locked
        public bool IsFirstLogin { get; set; } // If this is the user's first login
        public string? ProfilePic { get; set; } // If this is the user's first login

    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string TwoFactorCode { get; set; }
        public string TwoFactorRecoveryCode { get; set; }
    }

}

