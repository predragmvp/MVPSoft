using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace coderush.Models
{
    public class JobApplication : Base
    {
        public string JobApplicationId { get; set; }

        //basic info
        [Required]      
        public string Name { get; set; }
     
        [Required]
        [EmailAddress]
        public string Email { get; set; }
     
        [Required]      
        public string Description { get; set; }

        public string ResumePath { set; get; }

        [NotMapped]
        public IFormFile Resume { set; get; }

        [Required]     
        public string JobPostId { get; set; }
        public JobPost JobPost { get; set; }

    }
}
