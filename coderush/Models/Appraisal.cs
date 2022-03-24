using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //appraisal entity
    public class Appraisal : Base
    {
        public string AppraisalId { get; set; }

        [Required]
        [Display(Name = "Appraisal Name")]
        public string AppraisalName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Approved?")]
        public bool IsApproved { get; set; }

        public AppraisalType AppraisalType { get; set; }

        [Required]
        [Display(Name = "Appraisal Type")]
        public string AppraisalTypeId { get; set; }

        [Required]
        [Display(Name = "Submit Date")]
        public DateTime SubmitDate { get; set; }

        public Employee OnBehalf { get; set; }

        [Required]
        [Display(Name = "OnBehalf")]
        public string OnBehalfId { get; set; }
        
    }
}
