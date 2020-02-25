using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Comparison Page", 
        GUID = "73fb146b-ff08-4e97-8b21-dedafa6a121e", 
        Description = "The page used to compare 2 variants.",
        AvailableInEditMode = false,
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-29.png")]
    public class ComparisonPage : FoundationPageData
    {
        
    }
}