using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PayrollLineReimburse : Base
    {
        public string PayrollLineReimburseId { get; set; }
        public Payroll Payroll { get; set; }
        [Required]
        [Display(Name = "Payroll")]
        public string PayrollId { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        //expense type 
        public ExpenseType ExpenseType { get; set; }
        [Required]
        [Display(Name = "Expense Type")]
        public string ExpenseTypeId { get; set; }
        //amount of money
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
