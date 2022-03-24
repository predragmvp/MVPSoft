using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //type of leave
    public class LeaveType : Base
    {
        public string LeaveTypeId { get; set; }
        [Required]
        [Display(Name = "Leave Type Name")]
        public string Name { get; set; }
        [Display(Name = "Leave Type Description")]
        public string Description { get; set; }
    }
}
