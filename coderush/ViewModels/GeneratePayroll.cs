using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.ViewModels
{
    public class GeneratePayroll
    {
        [Required]
        public DateTime Periode { get; set; }
        public bool IsApproved { get; set; }
        public bool IsPaid { get; set; }
    }
}
