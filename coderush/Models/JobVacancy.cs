using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    // job vacancy or job opening campaign
    public class JobVacancy : Base
    {
        public string JobVacancyId { get; set; }
        [Required]
        [Display(Name = "Job Vacancy Name")]
        public string Name { get; set; }
        [Display(Name = "Job Vacancy Description")]
        public string Description { get; set; }
    }
}
