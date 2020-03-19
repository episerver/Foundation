using EPiServer.DataAnnotations;
using Geta.EpiCategories;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Categories
{
    [ContentType(
        GUID = "A9BBD7FC-27C5-4718-890A-E28ACBE5EE26",
        DisplayName = "Standard Category",
        Description = "Used to categorize content")]
    public class StandardCategory : CategoryData, IFoundationContent
    {
        [CultureSpecific]
        [Display(Name = "Hide site header", GroupName = CmsTabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site footer", GroupName = CmsTabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }
    }
}
