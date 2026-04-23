using EPiServer.Core.Routing;
using EPiServer.Core.Routing.Pipeline;

namespace Foundation.Features.Locations
{
    // EPiServer.Find removed: partial routing disabled.
    // UrlResolverContext.RemainingPath and GetNextRemainingSegment APIs were also removed in CMS 13.
    // This class compiles but is not registered, so it has no runtime effect.
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
            return null;
        }
    }
}
