using System.ComponentModel.DataAnnotations;

namespace coderush.Models
{
    //type of asset
    public class AssetType : Base
    {
        public string AssetTypeId { get; set; }
        [Required]
        [Display(Name = "Asset Type Name")]
        public string Name { get; set; }
        [Display(Name = "Asset Type Description")]
        public string Description { get; set; }
    }
}
