namespace SmartEmpAPI.Models
{
    public class EmployeeRequest
    {
        public int EmployeeId { get; set; } = 0;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class EmployeeAddUpdateRequest
    {
        public int? EmployeeID { get; set; } // Nullable for new employees (EmployeeID may be null for new records)
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DesignationID { get; set; }
        public int DepartmentID { get; set; }
        public string Email { get; set; }
        public string PersonalEmail { get; set; }
        public string CNIC { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; } // Nullable for optional dates
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime? JoiningDate { get; set; } // Nullable for optional dates
        public int? ShiftID { get; set; } // Nullable for optional shift ID
        public int? TeamID { get; set; } // Nullable for optional team ID
        public int ProfileID { get; set; } // Nullable for optional profile ID
        public string HighestDegree { get; set; }
        public string Institution { get; set; }
        public int? YearOfGraduation { get; set; } // Nullable for optional year
        public string Major { get; set; }
        public bool IsOnProbation { get; set; }
        public DateTime? ProbationEndDate { get; set; } // Nullable for optional probation end date
        public string EmploymentType { get; set; }
        public string WorkLocation { get; set; }
        public int? ReportingManagerID { get; set; } // Nullable for optional reporting manager
        public string BloodGroup { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? EmergencyContactRelationship { get; set; }
        public string? HealthCondition { get; set; }
        public string? DisabilityStatus { get; set; }
        public string? Medications { get; set; }
        public string? NumberOfDependents { get; set; }
        public string ProfilePic { get; set; }  // Base64 string
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; } // Nullable for update scenarios
        public string? Password { get; set; }
    }

}
