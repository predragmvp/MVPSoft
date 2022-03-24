using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //information entity
    public class Information : Base
    {
        public string InformationId { get; set; }

        [Required]
        [Display(Name = "Information Name")]
        public string InformationName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        public InformationType InformationType { get; set; }

        [Required]
        [Display(Name = "Information Type")]
        public string InformationTypeId { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        
        [Display(Name = "External Link")]
        public string ExternalLink { get; set; }

    }
}
