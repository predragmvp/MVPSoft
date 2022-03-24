using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class Employee : Base
    {
        public string EmployeeId { get; set; }

        //basic info

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Street Address1")]
        public string Address1 { get; set; }
        [Display(Name = "Street Address2")]
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "State / Province")]
        public string StateProvince { get; set; }
        [Display(Name = "ZipCode / Postal Code")]
        public string ZipCode { get; set; }
        public string Country { get; set; }


        //staff info

        [Display(Name = "Employee ID Number")]
        public string EmployeeIDNumber { get; set; }
        [Required]
        [Display(Name = "Designation")]
        public string DesignationId { get; set; }
        public Designation Designation { get; set; }
        [Required]
        [Display(Name = "Department")]
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
        [Required]
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; }
        [Display(Name = "Leaving Date")]
        public DateTime? LeavingDate { get; set; }
        [Display(Name = "Supervisor")]
        public string SupervisorId { get; set; }
        public Employee Supervisor { get; set; }


        //salary info

        [Required]
        [Display(Name = "Basic Salary")]
        public Decimal BasicSalary { get; set; }
        [Required]
        [Display(Name = "Un Paid Leave Per Day")]
        public Decimal UnpaidLeavePerDay { get; set; }
        [Display(Name = "Benefit Package Template")]
        public string BenefitTemplateId { get; set; }
        public BenefitTemplate BenefitTemplate { get; set; }


        //bank account info

        [Required]
        [Display(Name = "Account Title")]
        public string AccountTitle { get; set; }
        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }

        //system user account
        [Display(Name = "System Application User")]
        public string SystemUserId { get; set; }       
        public ApplicationUser SystemUser { get; set; }
    }
}
