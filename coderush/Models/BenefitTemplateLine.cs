using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    public class BenefitTemplateLine : Base
    {
       
        public string BenefitTemplateLineId { get; set; }
        [Required]
        [Display(Name = "Benefit Template")]
        public string BenefitTemplateId { get; set; }
        public BenefitTemplate BenefitTemplate { get; set; }
        [Required]
        [Display(Name = "Line Description")]
        public string Description { get; set; }

        //allowances
        public AllowanceType AllowanceType { get; set; }     
        [Display(Name = "Allowance")]
        public string AllowanceTypeId { get; set; }

        //deductions (should be minus)
        public DeductionType DeductionType { get; set; }      
        [Display(Name = "Deduction")]
        public string DeductionTypeId { get; set; }      

        //amount of money
        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}
