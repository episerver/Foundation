using EPiServer.Core.Routing;
using EPiServer.Core.Routing.Pipeline;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;

namespace Foundation.Features.Locations
{
    public class TagsPartialRouting : IPartialRouter<TagPage.TagPage, TagPage.TagPage>
    {
        public PartialRouteData GetPartialVirtualPath(TagPage.TagPage content, UrlGeneratorContext requestContext)
        {
            return new PartialRouteData
            {
                BasePathRoot = content.ContentLink,
                PartialVirtualPath = ""
            };
        }

        public object RoutePartial(TagPage.TagPage content, UrlResolverContext urlResolverContext)
        {
            var continentPart = urlResolverContext.GetNextRemainingSegment(urlResolverContext.RemainingPath);
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

                urlResolverContext.RouteValues.Add("Continent", continent);
                urlResolverContext.RemainingPath = continentPart.Remaining;
                return content;
            }

            return null;
        }
    }
}