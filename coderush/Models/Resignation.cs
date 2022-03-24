using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //resignation of an employee
    public class Resignation : Base
    {
        public string ResignationId { get; set; }
        [Required]
        [Display(Name = "Resignation Name")]
        public string Name { get; set; }
        [Display(Name = "Resignation Description")]
        public string Description { get; set; }
    }
}
