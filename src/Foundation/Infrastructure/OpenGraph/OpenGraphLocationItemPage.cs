using Boilerplate.Web.Mvc.OpenGraph;
using Foundation.Infrastructure.OpenGraph.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Infrastructure.OpenGraph
{
    public class OpenGraphLocationItemPage : OpenGraphFoundationPageData
    {
        public OpenGraphLocationItemPage(string title, OpenGraphImage image, string url = null) : base(title, image, url)
        {
        }

        public override string Namespace => "website: http://ogp.me/ns/article#";

        public override OpenGraphType Type => OpenGraphType.Article;

        public IEnumerable<string> Tags { get; set; }

        public override void ToString(StringBuilder stringBuilder)
        {
            if (stringBuilder is null)
            {
                throw new ArgumentNullException(nameof(stringBuilder));
            }

            base.ToString(stringBuilder);

            stringBuilder.AppendMetaPropertyContentIfNotNull("article:tag", Tags);
        }
    }
}