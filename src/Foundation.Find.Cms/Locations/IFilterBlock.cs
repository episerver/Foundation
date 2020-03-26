using EPiServer.Find;
using Foundation.Find.Cms.Models.Pages;
using System.Collections.Specialized;

namespace Foundation.Find.Cms.Locations
{
    public interface IFilterBlock
    {
        string FilterTitle { get; set; }

        string AllConditionText { get; set; }

        ITypeSearch<LocationItemPage> AddFilter(ITypeSearch<LocationItemPage> query);

        ITypeSearch<LocationItemPage> ApplyFilter(ITypeSearch<LocationItemPage> query, NameValueCollection filters);
    }
}
