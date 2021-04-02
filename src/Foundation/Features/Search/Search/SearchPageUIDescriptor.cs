using EPiServer.Shell;

namespace Foundation.Features.Search.Search
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