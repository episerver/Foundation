using EPiServer.DataAnnotations;
using EPiServer.Find;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Features.Locations.Blocks
{
    [ContentType(DisplayName = "Filter Temperatures Block",
        GUID = "28629b4b-9475-4c44-9c15-31961391f166",
        Description = "Temperature slider for locations",
        GroupName = TabNames.Location)]
    [ImageUrl("/icons/cms/blocks/map.png")]
    [AvailableContentTypes(Include = new Type[] { typeof(LocationListPage.LocationListPage) })]
    public class FilterTemperaturesBlock : FoundationBlockData, IFilterBlock
    {
        [CultureSpecific]
        [Display(Name = "Filter title")]
        public virtual string FilterTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "All condition text")]
        public virtual string AllConditionText { get; set; }

        public ITypeSearch<LocationItemPage.LocationItemPage> AddFilter(ITypeSearch<LocationItemPage.LocationItemPage> query) => query;

        public ITypeSearch<LocationItemPage.LocationItemPage> ApplyFilter(ITypeSearch<LocationItemPage.LocationItemPage> query, IQueryCollection filters)
        {
            var filterString = filters["t"];
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                var temperatures = filterString.ToList();
                if (int.TryParse(temperatures.First(), out var f) && int.TryParse(temperatures.Last(), out var t) && f <= t && f >= -20 && t <= 40)
                {
                    query = query.Filter(x => x.AvgTempDbl.InRange(f, t));
                }
            }
            return query;
        }
    }
}