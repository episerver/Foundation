using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Find;
using Foundation.Cms.Blocks;
using Foundation.Find.Cms.Locations;
using Foundation.Find.Cms.Models.Pages;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Foundation.Find.Cms.Models.Blocks
{
    [ContentType(
        DisplayName = "Filter Temperatures Block",
        GUID = "28629b4b-9475-4c44-9c15-31961391f166",
        Description = "Temperature slider for locations",
        GroupName = FindTabs.Location)]
    [ImageUrl("~/assets/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage) })]
    public class FilterTemperaturesBlock : FoundationBlockData, IFilterBlock
    {
        public virtual string FilterTitle { get; set; }

        public ITypeSearch<LocationItemPage> AddFilter(ITypeSearch<LocationItemPage> query)
        {
            return query;
        }

        public ITypeSearch<LocationItemPage> ApplyFilter(ITypeSearch<LocationItemPage> query, NameValueCollection filters)
        {
            var filterString = filters["t"];
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                int f, t;
                var temperatures = filterString.Split(',').ToList();
                if (Int32.TryParse(temperatures.First(), out f) && Int32.TryParse(temperatures.Last(), out t) && f <= t && f >= -20 && t <= 40)
                {
                    query = query.Filter(x => x.AvgTempDbl.InRange(f, t));
                }
            }
            return query;
        }
    }
}