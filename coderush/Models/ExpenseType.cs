using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //type of expense
    public class ExpenseType : Base
    {
        public string ExpenseTypeId { get; set; }
        [Required]
        [Display(Name = "Expense Type Name")]
        public string Name { get; set; }
        [Display(Name = "Expense Type Description")]
        public string Description { get; set; }
    }
}
