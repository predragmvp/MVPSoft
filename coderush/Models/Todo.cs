using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //todo class entity for simple todo app example
    public class Todo : Base
    {
        public string TodoId { get; set; }
        [Required]
        [Display(Name = "Todo Item")]
        public string TodoItem { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Is Done?")]
        public bool IsDone { get; set; }        
        public TodoType TodoType { get; set; }
        [Required]
        [Display(Name = "Todo Type")]
        public string TodoTypeId { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public Employee OnBehalf { get; set; }
        [Required]
        [Display(Name = "OnBehalf")]
        public string OnBehalfId { get; set; }
    }
}
