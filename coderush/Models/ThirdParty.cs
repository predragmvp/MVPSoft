using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    // head hunter, agency, job board company or other that act as third party on recruitment process
    public class ThirdParty : Base
    {
        public string ThirdPartyId { get; set; }
        [Required] 
        [Display(Name = "Third Party Name")]
        public string Name { get; set; }
        [Display(Name = "Third Party Description")]
        public string Description { get; set; }
    }
}
