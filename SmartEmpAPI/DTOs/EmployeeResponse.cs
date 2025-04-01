using SmartEmpAPI.DTOs;

namespace SmartEmpAPI.Models
{
    namespace SmartEmpAPI.Models
    {
        public class EmployeeResponse
        {
            public Response Resp  { get; set; }
            public List<Dictionary<string, object>> EmployeeData { get; set; }
        }
         public class PasswordResetResponse
        {
            public Response Resp { get; set; }
            public string GeneratedPassword { get; set; }

        }

        public class EmployeeDetails
        {
            public int EmployeeID { get; set; }
            public int ProfileID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Personal_Email { get; set; }
            public string CNIC { get; set; }
            public string Phone { get; set; }
            public string Gender { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string Country { get; set; }
            public string MaritalStatus { get; set; }
            public DateTime JoiningDate { get; set; }
            public int? DepartmentID { get; set; }
            public int? DesignationID { get; set; }
            public int? ShiftID { get; set; }
            public int? TeamID { get; set; }
            public string Highest_degree { get; set; }
            public string Institution { get; set; }
            public int? Year_of_graduation { get; set; }
            public string Major { get; set; }
            public bool IsOnProbation { get; set; }
            public DateTime? ProbationEndDate { get; set; }
            public string EmploymentType { get; set; }
            public string WorkLocation { get; set; }
            public int? ReportingManagerID { get; set; }
            public string BloodGroup { get; set; }
            public string Emergency_contact_name { get; set; }
            public string Emergency_contact_number { get; set; }
            public string Emergency_contact_relationship { get; set; }
            public string Health_condition { get; set; }
            public string Disability_status { get; set; }
            public string Medications { get; set; }
            public int? Number_of_dependents { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public byte[] ProfilePic { get; set; }
            public string Designation { get; set; }

        }
    }
  
   
}
