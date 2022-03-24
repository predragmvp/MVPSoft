using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class Layoff : Base
    {
        public string LayoffId { get; set; }
        [Required]
        [Display(Name = "Layoff Name")]
        public string Name { get; set; }
        [Display(Name = "Layoff Description")]
        public string Description { get; set; }
    }
}
