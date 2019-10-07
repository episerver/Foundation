using EPiServer.Find.UnifiedSearch;
using Foundation.Find.Cms.ViewModels;

namespace Foundation.Find.Cms
{
    public interface ICmsSearchService
    {
        ContentSearchViewModel SearchContent(CmsFilterOptionViewModel filterOptions);
    }
}
