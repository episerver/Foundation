using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Routing;
using EPiServer.Web.Routing.Segments;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Locations
{
    public class LocationsPartialRouting : IPartialRouter<LocationItemPage.LocationItemPage, TagPage.TagPage>
    {
        public PartialRouteData GetPartialVirtualPath(TagPage.TagPage content, string language, System.Web.Routing.RouteValueDictionary routeValues, System.Web.Routing.RequestContext requestContext)
        {
            return new PartialRouteData
            {
                BasePathRoot = content.ContentLink,
                PartialVirtualPath = ""
            };
        }

        public object RoutePartial(LocationItemPage.LocationItemPage content, SegmentContext segmentContext)
        {
            var elements = segmentContext.RemainingPath.Split('/');
            segmentContext.RemainingPath = string.Empty;

            TagPage.TagPage cp = null;
            var catpages = SearchClient.Instance.Search<TagPage.TagPage>().Take(100).GetContentResult().ToList();
            var continents = SearchClient.Instance.Search<LocationItemPage.LocationItemPage>()
                .TermsFacetFor(f => f.Continent)
                .Take(0)
                .GetContentResult()
                .TermsFacetFor(f => f.Continent)
                .Terms.Select(tc => tc.Term.ToLower())
                .ToList();

            var additionalcats = new List<string>();

            foreach (var s in elements)
            {
                var k = s.ToLower();
                if (continents.Contains(k))
                {
                    segmentContext.SetCustomRouteData("Continent", s);
                }
                else if (cp == null)
                {
                    cp = catpages.FirstOrDefault(c => c.URLSegment.ToLower() == k);
                }
                else
                {
                    var cat = catpages.FirstOrDefault(c => c.URLSegment.ToLower() == k);
                    if (cat == null)
                    {
                        return null;
                    }

                    additionalcats.Add(cat.Name);
                }

                //if s is category and category page is null, set category page.
                //if s is continent, set continent
                //if s is another category, set other category
            }
            if (additionalcats.Count > 0)
            {
                segmentContext.SetCustomRouteData("Category", string.Join(",", additionalcats.ToArray()));
            }

            return cp;
        }
    }
}