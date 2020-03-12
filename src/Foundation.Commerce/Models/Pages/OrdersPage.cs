using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Organization Orders Page",
        GUID = "3c436a14-38d1-4fd1-ab37-15f0848cfa24",
        Description = "Page to manage an organizations's orders",
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-15.png")]
    public class OrdersPage : FoundationPageData
    {
    }
}