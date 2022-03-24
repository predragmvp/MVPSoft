using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class Base
    {
        public string CreatedById { get; set; }
        [Display(Name = "Created By")]
        public ApplicationUser CreatedBy { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAtUtc { get; set; }
        public string UpdatedById { get; set; }
        [Display(Name = "Updated By")]
        public ApplicationUser UpdatedBy { get; set; }
        [Display(Name = "Updated At")]
        public DateTime UpdatedAtUtc { get; set; }
    }
}
