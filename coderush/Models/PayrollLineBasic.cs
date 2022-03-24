using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PayrollLineBasic : Base
    {
        public string PayrollLineBasicId { get; set; }
        public Payroll Payroll { get; set; }
        [Required]
        [Display(Name = "Payroll")]
        public string PayrollId { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //amount of money
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
