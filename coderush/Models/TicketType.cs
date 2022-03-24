using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //type of ticket
    public class TicketType : Base
    {
        public string TicketTypeId { get; set; }
        [Required]
        [Display(Name = "Ticket Type Name")]
        public string Name { get; set; }
        [Display(Name = "Ticket Type Description")]
        public string Description { get; set; }
    }
}
