using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PayrollLineDeduction : Base
    {
        public string PayrollLineDeductionId { get; set; }
        public Payroll Payroll { get; set; }
        [Required]
        [Display(Name = "Payroll")]
        public string PayrollId { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //deduction
        public DeductionType DeductionType { get; set; }
        [Required]
        [Display(Name = "Deduction Type")]
        public string DeductionTypeId { get; set; }
        //amount of money
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
