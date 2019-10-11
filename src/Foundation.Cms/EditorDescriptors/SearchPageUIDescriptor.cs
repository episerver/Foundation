using EPiServer.Shell;
using Foundation.Cms.Pages;

namespace Foundation.Cms.EditorDescriptors
{
    /// <summary>
    /// Describes how the UI should appear for <see cref="SearchResultPage"/> content.
    /// </summary>
    [UIDescriptorRegistration]
    public class SearchPageUIDescriptor : UIDescriptor<SearchResultPage>
    {
        public SearchPageUIDescriptor()
            : base("epi-iconSearch epi-icon--primary")
        {
        }
    }
}
