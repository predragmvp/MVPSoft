using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //type of todo
    public class TodoType : Base
    {
        public string TodoTypeId { get; set; }
        [Required]
        [Display(Name = "Todo Type Name")]
        public string Name { get; set; }
        [Display(Name = "Todo Type Description")]
        public string Description { get; set; }
    }
}
