using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Routing;

namespace Foundation.Features.Locations
{
    public class TagsPartialRouting : IPartialRouter<TagPage.TagPage, TagPage.TagPage>
    {
        public PartialRouteData GetPartialVirtualPath(TagPage.TagPage content, string language, System.Web.Routing.RouteValueDictionary routeValues, System.Web.Routing.RequestContext requestContext)
        {
            return new PartialRouteData
            {
                BasePathRoot = content.ContentLink,
                PartialVirtualPath = ""
            };
        }

        public object RoutePartial(TagPage.TagPage content, EPiServer.Web.Routing.Segments.SegmentContext segmentContext)
        {
            var continentPart = segmentContext.GetNextValue(segmentContext.RemainingPath);
            if (!string.IsNullOrEmpty(continentPart.Next))
            {
                var continent = continentPart.Next;
                //Check continent exists for this category
                var mcount = SearchClient.Instance.Search<LocationItemPage.LocationItemPage>()
                    .Filter(dp => dp.TagString().Match(content.Name)).Filter(dp => dp.Continent.MatchCaseInsensitive(continent))
                    .Take(0).GetContentResult().TotalMatching;

                if (mcount == 0)
                {
                    return null;
                }

                segmentContext.SetCustomRouteData("Continent", continent);
                segmentContext.RemainingPath = continentPart.Remaining;
                return content;
            }
            return null;
        }
    }
}