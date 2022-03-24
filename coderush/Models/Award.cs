using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //Award entity
    public class Award : Base
    {
        public string AwardId { get; set; }

        [Required]
        [Display(Name = "Award Name")]
        public string AwardName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Approved?")]
        public bool IsApproved { get; set; }

        public AwardType AwardType { get; set; }

        [Required]
        [Display(Name = "Award Type")]
        public string AwardTypeId { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public Employee AwardRecipient { get; set; }

        [Required]
        [Display(Name = "Award Recipient")]
        public string AwardRecipientId { get; set; }
        
    }
}
