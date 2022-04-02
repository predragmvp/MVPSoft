using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class JobPost : Base
    {
        public string JobPostId { get; set; }

        [Required]
        [Display(Name = "Job Titile")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
       
        [Display(Name = "Is Active ? ")]
        public bool IsActive{ get; set; }

        //system user account
        [Display(Name = "System Application User")]
        public string SystemUserId { get; set; }       
        public ApplicationUser SystemUser { get; set; }
    }
}
