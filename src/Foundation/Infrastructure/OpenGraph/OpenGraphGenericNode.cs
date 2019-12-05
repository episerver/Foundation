using Boilerplate.Web.Mvc.OpenGraph;
using Foundation.Infrastructure.OpenGraph.Extensions;
using System;
using System.Text;

namespace Foundation.Infrastructure.OpenGraph
{
    public class OpenGraphGenericNode : OpenGraphMetadata
    {
        public OpenGraphGenericNode(string title, OpenGraphImage image, string url = null) : base(title, image, url)
        {
        }

        public override string Namespace => "website: http://ogp.me/ns/website#";

        public override OpenGraphType Type => OpenGraphType.Website;

        public string ContentType { get; set; }
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
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:published_time", PublishedTime);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:modified_time", ModifiedTime);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:expiration_time", ExpirationTime);
        }
    }
}