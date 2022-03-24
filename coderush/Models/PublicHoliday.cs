using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //public holiday registration per year
    public class PublicHoliday : Base
    {
        public string PublicHolidayId { get; set; }
        [Required]
        [Display(Name = "Public Holiday Name")]
        public string Name { get; set; }
        [Display(Name = "Public Holiday Description")]
        public string Description { get; set; }


        //lines
        public List<PublicHolidayLine> Lines { get; set; } = new List<PublicHolidayLine>();
    }
}
