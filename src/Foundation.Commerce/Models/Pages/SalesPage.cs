using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Sales Page",
        GUID = "9f6352bc-eea4-416a-bf76-144037c7d3db",
        Description = "Show all items on sale",
        GroupName = CommerceTabNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-21.png")]
    public class SalesPage : FoundationPageData
    {

    }
}