using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Cms.SiteSettings.Pages
{
    [ContentType(DisplayName = "Setting Node", 
        GUID = "5f5e31fc-3694-41c1-8f31-c50477b0f121", 
        Description = "Node of settings pages",
        GroupName = CmsGroupNames.Settings)]
    [AvailableContentTypes(Availability.Specific,
        Include = new [] { typeof(SettingNode), typeof(SettingsBasePage) },
        ExcludeOn = new[] { typeof(FoundationPageData) })]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-settings-node.png")]
    public class SettingNode : PageData
    {
    }
}