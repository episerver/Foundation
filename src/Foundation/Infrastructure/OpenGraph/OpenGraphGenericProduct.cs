using Boilerplate.Web.Mvc.OpenGraph;
using Foundation.Infrastructure.OpenGraph.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foundation.Infrastructure.OpenGraph
{
    public class OpenGraphGenericProduct : OpenGraphMetadata
    {
        public OpenGraphGenericProduct(string title, OpenGraphImage image, string url = null) : base(title, image, url)
        {
        }

        public override string Namespace => "product: http://ogp.me/ns/product#";

        public override OpenGraphType Type => OpenGraphType.Product;

        public string Brand { get; set; }
        public IEnumerable<string> Category { get; set; }
        public string PriceAmount { get; set; }
        public string PriceCurrency { get; set; }

        public override void ToString(StringBuilder stringBuilder)
        {
            if (stringBuilder is null)
            {
                throw new ArgumentNullException(nameof(stringBuilder));
            }

            base.ToString(stringBuilder);

            stringBuilder.AppendMetaPropertyContentIfNotNull("article:brand", Brand);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:category", Category);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:price:amount", PriceAmount);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:price:currency", PriceCurrency);
        }
    }
}