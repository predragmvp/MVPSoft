using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PayrollLineUnpaidLeave : Base
    {
        public string PayrollLineUnpaidLeaveId { get; set; }
        public Payroll Payroll { get; set; }
        [Display(Name = "Payroll")]
        public string PayrollId { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //unpaid leave 
        public Leave Leave { get; set; }
        [Required]
        [Display(Name = "Unpaid Leave")]
        public string LeaveId { get; set; }
        [Required]
        [Display(Name = "Days")]
        public int Days { get; set; }
        [Required]
        [Display(Name = "Unpaid Per Day")]
        public decimal UnpaidPerDay { get; set; }
        //amount of money
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
