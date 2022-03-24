using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //Leave entity
    public class Leave : Base
    {
        public string LeaveId { get; set; }

        [Required]
        [Display(Name = "Leave Name")]
        public string LeaveName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Emergency Call")]
        public string EmergencyCall { get; set; }

        [Display(Name = "Is Approved?")]
        public bool IsApproved { get; set; }

        public LeaveType LeaveType { get; set; }

        [Required]
        [Display(Name = "Leave Type")]
        public string LeaveTypeId { get; set; }

        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        public Employee OnBehalf { get; set; }

        [Required]
        [Display(Name = "OnBehalf")]
        public string OnBehalfId { get; set; }

        [Required]
        [Display(Name = "Is Paid Leave?")]
        public bool IsPaidLeave { get; set; }


    }
}
