using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class DeductionType : Base
    {
        public string DeductionTypeId { get; set; }
        [Required]
        [Display(Name = "Deduction Type Name")]
        public string Name { get; set; }
        [Display(Name = "Deduction Type Description")]
        public string Description { get; set; }
    }
}
