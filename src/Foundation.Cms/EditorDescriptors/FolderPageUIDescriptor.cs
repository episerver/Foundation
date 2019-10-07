using EPiServer.Shell;
using Foundation.Cms.Pages;

namespace Foundation.Cms.EditorDescriptors
{
    /// <summary>
    /// Describes how the UI should appear for <see cref="FolderPage"/> content.
    /// </summary>
    [UIDescriptorRegistration]
    public class FolderPageUIDescriptor : UIDescriptor<FolderPage>
    {
        public FolderPageUIDescriptor()
            : base(ContentTypeCssClassNames.Folder)
        {
        }
    }
}
