using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //on boarding for new staff listing all the preps and tools
    public class OnBoarding : Base
    {
        public string OnBoardingId { get; set; }
        [Required]
        [Display(Name = "OnBoarding Name")]
        public string Name { get; set; }
        [Display(Name = "OnBoarding Description")]
        public string Description { get; set; }
    }
}
