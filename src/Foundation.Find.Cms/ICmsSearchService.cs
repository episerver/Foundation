using EPiServer.Core;
using EPiServer.Find;
using Foundation.Find.Cms.ViewModels;
using Geta.EpiCategories;
using System.Collections.Generic;

namespace Foundation.Find.Cms
{
    public interface ICmsSearchService
    {
        ContentSearchViewModel SearchContent(CmsFilterOptionViewModel filterOptions);
        CategorySearchResults SearchByCategory(Pagination pagination);
        ITypeSearch<T> FilterByCategories<T>(ITypeSearch<T> query, IEnumerable<ContentReference> categories) where T : ICategorizableContent;
    }
}
