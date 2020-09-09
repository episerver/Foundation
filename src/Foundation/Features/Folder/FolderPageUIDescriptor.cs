using EPiServer.Shell;

namespace Foundation.Features.Folder
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