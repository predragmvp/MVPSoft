using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //type of information
    public class InformationType : Base
    {
        public string InformationTypeId { get; set; }
        [Required]
        [Display(Name = "Information Type Name")]
        public string Name { get; set; }
        [Display(Name = "Information Type Description")]
        public string Description { get; set; }
    }
}
