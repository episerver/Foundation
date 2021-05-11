using Boxed.AspNetCore.TagHelpers.OpenGraph;

namespace Foundation.Infrastructure.OpenGraph
{
    public class OpenGraphHomePage : OpenGraphFoundationPageData
    {
        public OpenGraphHomePage(string title, OpenGraphImage image, string url = null) : base(title, image, url)
        {
        }

        public override string Namespace => "website: http://ogp.me/ns/website#";

        public override OpenGraphType Type => OpenGraphType.Website;
    }
}