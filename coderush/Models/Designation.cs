using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    // official position of an employee
    public class Designation : Base
    {
     
        public string DesignationId { get; set; }
        [Required]
        [Display(Name = "Designation Name")]
        public string Name { get; set; }
        [Display(Name = "Designation Description")]
        public string Description { get; set; }
    }
}
