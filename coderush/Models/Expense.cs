using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //Expense entity
    public class Expense : Base
    {
        public string ExpenseId { get; set; }

        [Required]
        [Display(Name = "Expense Name")]
        public string ExpenseName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        

        [Display(Name = "Is Approved?")]
        public bool IsApproved { get; set; }

        public ExpenseType ExpenseType { get; set; }

        [Required]
        [Display(Name = "Expense Type")]
        public string ExpenseTypeId { get; set; }

        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Required]
        [Display(Name = "Expense Amount")]
        public decimal ExpenseAmount { get; set; }

        [Display(Name = "Is Cash Advance?")]
        public bool IsCashAdvance { get; set; }

        public Employee OnBehalf { get; set; }

        [Required]
        [Display(Name = "OnBehalf")]
        public string OnBehalfId { get; set; }


    }
}
