using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms.Facets.Config
{
    public class FacetFilterProductConfigurationItem : FacetFilterConfigurationItem
    {
        [Display(
            Name = "Attribute as Filter (required)",
            Description = "Attribute to be used as a filter",
            Order = 1)]
        [Required]
        [SelectOneEnum(typeof(FacetProductFieldName))]
        public override string FieldName { get; set; }
    }
}
