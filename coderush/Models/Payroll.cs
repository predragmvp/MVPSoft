using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //Payroll entity
    public class Payroll : Base
    {
        public string PayrollId { get; set; }

        [Required]
        [Display(Name = "Payroll Name")]
        public string PayrollName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        

        [Display(Name = "Is Approved?")]
        public bool IsApproved { get; set; }

        [Display(Name = "Is Paid?")]
        public bool IsPaid { get; set; }


        [Required]
        [Display(Name = "Periode")]
        public DateTime Periode { get; set; }
        

        public Employee OnBehalf { get; set; }

        [Required]
        [Display(Name = "OnBehalf")]
        public string OnBehalfId { get; set; }

        //lines
        public List<PayrollLineBasic> LinesBasic { get; set; } = new List<PayrollLineBasic>();
        public List<PayrollLineAllowance> LinesAllowance { get; set; } = new List<PayrollLineAllowance>();
        public List<PayrollLineDeduction> LinesDeduction { get; set; } = new List<PayrollLineDeduction>();
        public List<PayrollLineCashAdvance> LinesCashAdvance { get; set; } = new List<PayrollLineCashAdvance>();
        public List<PayrollLineReimburse> LinesReimburse { get; set; } = new List<PayrollLineReimburse>();
        public List<PayrollLineUnpaidLeave> LinesUnpaidLeave { get; set; } = new List<PayrollLineUnpaidLeave>();
    }
}
