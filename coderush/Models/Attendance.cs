using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class Attendance : Base
    {
        public string AttendanceId { get; set; }
        [Required]
        [Display(Name = "Attendance Name")]
        public string AttendanceName { get; set; }

        [Display(Name = "Attendance Description")]
        public string Description { get; set; }

        public Employee OnBehalf { get; set; }

        [Required]
        [Display(Name = "OnBehalf")]
        public string OnBehalfId { get; set; }

        [Required]
        [Display(Name = "Clock")]
        public DateTime Clock { get; set; }
    }
}
