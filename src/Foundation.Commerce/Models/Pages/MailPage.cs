using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Mail Page",
        GUID = "73bc5587-eef3-4844-be9d-0c90d081e2e4",
        Description = "The template mail page.",
        GroupName = CmsTabNames.Account)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-26.png")]
    public class MailPage : MailBasePage
    {
    }
}