using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace coderush.Models
{
    //asset entity
    public class Asset : Base
    {
        public string AssetId { get; set; }

        [Required]
        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        public AssetType AssetType { get; set; }

        [Required]
        [Display(Name = "Asset Type")]
        public string AssetTypeId { get; set; }

        [Required]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }

        public Employee UsedBy { get; set; }
        
        [Display(Name = "UsedBy")]
        public string UsedById { get; set; }
        
    }
}
