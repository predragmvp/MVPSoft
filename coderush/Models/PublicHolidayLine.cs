using System;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class PublicHolidayLine : Base
    {
        public string PublicHolidayLineId { get; set; }
        [Required]
        [Display(Name = "Public Holiday")]
        public string PublicHolidayId { get; set; }
        public PublicHoliday PublicHoliday { get; set; }
        [Required]
        [Display(Name = "Line Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Public Holiday Date")]
        public DateTime PublicHolidayDate { get; set; }
        [Required]
        [Display(Name = "Public Holiday Year")]
        public int PublicHolidayYear { get; set; }
    }
}
