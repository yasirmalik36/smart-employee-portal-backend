namespace SmartEmpAPI.Models
{
    namespace SmartEmpAPI.Models
    {
        public class EmployeeResponse
        {
            public string StatusMessage { get; set; }
            public string StatusCode { get; set; }
            public EmployeeDetails EmployeeDetails { get; set; }
        }

        public class EmployeeDetails
        {
            public int EmployeeId { get; set; }
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string HighestDegree { get; set; }
            public string Institution { get; set; }
            public int? YearOfGraduation { get; set; }
            public string Major { get; set; }
            public string AdditionalCertifications { get; set; }
            public string LanguagesSpoken { get; set; }
            public string PreviousEmployer { get; set; }
            public string PreviousJobTitle { get; set; }
            public DateTime? HireDate { get; set; }
            public int? ProbationPeriod { get; set; }
            public string EmploymentStatus { get; set; }
            public string JobTitle { get; set; }
            public string Department { get; set; }
            public string TeamName { get; set; }
            public int? ManagerId { get; set; }
            public decimal Salary { get; set; }
            public string EmploymentType { get; set; }
            public string WorkLocation { get; set; }
            public int WorkingHoursPerWeek { get; set; }
            public DateTime? ContractStartDate { get; set; }
            public DateTime? ContractEndDate { get; set; }
            public string BankAccountNumber { get; set; }
            public string BankName { get; set; }
            public string NationalId { get; set; }
            public string PassportNumber { get; set; }
            public string FinanceNotes { get; set; }
            public string TaxIdentificationNumber { get; set; }
            public string SalaryAccountingCode { get; set; }
            public string RefereeName { get; set; }
            public string RefereeContact { get; set; }
            public string RefereeRelationship { get; set; }
            public string EmergencyContactName { get; set; }
            public string EmergencyContactNumber { get; set; }
            public string EmergencyContactRelationship { get; set; }
            public string HealthCondition { get; set; }
            public string DisabilityStatus { get; set; }
            public string Medications { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? TerminationDate { get; set; }
            public string ExitReason { get; set; }
            public string RehireStatus { get; set; }
            public string WorkEmail { get; set; }
            public string PersonalEmail { get; set; }
            public string MaritalStatus { get; set; }
            public int NumberOfDependents { get; set; }
            public string SocialSecurityNumber { get; set; }
            public bool IsActive { get; set; }
        }
    }

}
