using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PayrollLineAllowance : Base
    {
        public string PayrollLineAllowanceId { get; set; }
        public Payroll Payroll { get; set; }
        [Required]
        [Display(Name = "Payroll")]
        public string PayrollId { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //allowance
        public AllowanceType AllowanceType { get; set; }
        [Required]
        [Display(Name = "Allowance Type")]
        public string AllowanceTypeId { get; set; }
        //amount of money
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
