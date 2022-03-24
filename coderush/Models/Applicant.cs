using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //tracking applicants that apply to a job campaign
    public class Applicant : Base
    {
        public string ApplicantId { get; set; }
        [Required]
        [Display(Name = "Applicant Name")]
        public string Name { get; set; }
        [Display(Name = "Applicant Description")]
        public string Description { get; set; }
    }
}
