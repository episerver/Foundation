using Boilerplate.Web.Mvc.OpenGraph;
using Foundation.Infrastructure.OpenGraph.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Infrastructure.OpenGraph
{
    public class OpenGraphFoundationPageData : OpenGraphMetadata
    {
        public OpenGraphFoundationPageData(string title, OpenGraphImage image, string url = null) : base(title, image, url)
        {
        }

        public override string Namespace => "website: http://ogp.me/ns/article#";

        public override OpenGraphType Type => OpenGraphType.Article;

        public string ContentType { get; set; }
        public IEnumerable<string> Category { get; set; }
        public string Industry { get; set; }
        public string Author { get; set; }
        public DateTime? PublishedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime? ExpirationTime { get; set; }

        public override void ToString(StringBuilder stringBuilder)
        {
            if (stringBuilder is null)
            {
                throw new ArgumentNullException(nameof(stringBuilder));
            }

            base.ToString(stringBuilder);

            stringBuilder.AppendMetaPropertyContentIfNotNull("article:content_type", ContentType);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:category", Category);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:industry", Industry);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:published_time", PublishedTime);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:modified_time", ModifiedTime);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:expiration_time", ExpirationTime);
        }
    }
}