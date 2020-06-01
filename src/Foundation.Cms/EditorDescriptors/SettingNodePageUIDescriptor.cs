using EPiServer.Shell;
using Foundation.Cms.SiteSettings.Pages;

namespace Foundation.Cms.EditorDescriptors
{
    /// <summary>
    /// Describes how the UI should appear for <see cref="SettingNode"/> content.
    /// </summary>
    [UIDescriptorRegistration]
    public class SettingNodePageUIDescriptor : UIDescriptor<SettingNode>
    {
        public SettingNodePageUIDescriptor()
            : base(ContentTypeCssClassNames.Container)
        {
        }
    }
}
