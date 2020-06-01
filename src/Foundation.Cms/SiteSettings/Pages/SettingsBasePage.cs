using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Cms.SiteSettings.Pages
{
    [AvailableContentTypes(Availability = Availability.Specific,
        Exclude = new[] { typeof(PageData) },
        ExcludeOn = new[] { typeof(FoundationPageData), typeof(FolderPage) },
        IncludeOn = new[] { typeof(SettingNode) })]

    public abstract class SettingsBasePage : PageData
    {
    }
}