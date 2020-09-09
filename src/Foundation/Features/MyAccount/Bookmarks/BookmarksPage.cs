using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;

namespace Foundation.Features.MyAccount.Bookmarks
{
    [ContentType(DisplayName = "Bookmarks Page",
        GUID = "40E76908-6AA2-4CB7-8239-607D941DF3A6",
        Description = "This page displays list the different content that has been bookmarked belonging to an user",
        GroupName = SystemTabNames.Content,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-28.png")]
    public class BookmarksPage : FoundationPageData
    {
    }
}