using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Profile Page",
        GUID = "c03371fb-fc21-4a6e-8f79-68c400519145",
        Description = "Page to show and manage profile information",
        GroupName = SystemTabNames.Content,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class ProfilePage : FoundationPageData
    {

    }
}