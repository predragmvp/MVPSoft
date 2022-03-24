using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //division of a larger organization into parts with specific responsibility
    public class Department : Base
    {
        public string DepartmentId { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public string Name { get; set; }
        [Display(Name = "Department Description")]
        public string Description { get; set; }
    }
}
