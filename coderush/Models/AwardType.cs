using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //type of Award
    public class AwardType : Base
    {
        public string AwardTypeId { get; set; }
        [Required]
        [Display(Name = "Award Type Name")]
        public string Name { get; set; }
        [Display(Name = "Award Type Description")]
        public string Description { get; set; }
    }
}
