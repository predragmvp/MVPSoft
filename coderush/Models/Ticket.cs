using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    //ticket entity
    public class Ticket : Base
    {
        public string TicketId { get; set; }

        [Required]
        [Display(Name = "Ticket Name")]
        public string TicketName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Solve?")]
        public bool IsSolve { get; set; }
        
        [Display(Name = "Solution Note")]
        public string SolutionNote { get; set; }

        public TicketType TicketType { get; set; }

        [Required]
        [Display(Name = "Ticket Type")]
        public string TicketTypeId { get; set; }

        [Required]
        [Display(Name = "Submit Date")]
        public DateTime SubmitDate { get; set; }

        public Employee OnBehalf { get; set; }

        [Required]
        [Display(Name = "OnBehalf")]
        public string OnBehalfId { get; set; }
        
        public Employee Agent { get; set; }

       
        [Display(Name = "Agent")]
        public string AgentId { get; set; }

        public Ticket ParentTicketThread { get; set; }

        [Display(Name = "Parent Ticket Thread")]
        public string ParentTicketThreadId { get; set; }
    }
}
