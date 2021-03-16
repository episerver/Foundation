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

            stringBuilder.AppendMetaPropertyContent("og:title", Title);
            if (Type != OpenGraphType.Website)
            {
                stringBuilder.AppendMetaPropertyContent("og:type", Type.ToLowercaseString());
            }
            stringBuilder.AppendMetaPropertyContent("og:url", Url);
            foreach (OpenGraphMedia medium in Media)
            {
                stringBuilder.AppendMetaPropertyContent("og:image", medium.Url);
            }
            stringBuilder.AppendMetaPropertyContentIfNotNull("og:description", Description);
            stringBuilder.AppendMetaPropertyContentIfNotNull("og:site_name", SiteName);
            if (Determiner != 0)
            {
                stringBuilder.AppendMetaPropertyContent("og:determiner", Determiner.ToLowercaseString());
            }
            if (Locale != null)
            {
                stringBuilder.AppendMetaPropertyContent("og:locale", Locale);
                if (AlternateLocales != null)
                {
                    foreach (string alternateLocale in AlternateLocales)
                    {
                        stringBuilder.AppendMetaPropertyContent("og:locale:alternate", alternateLocale);
                    }
                }
            }
            if (SeeAlso != null)
            {
                foreach (string item in SeeAlso)
                {
                    stringBuilder.AppendMetaPropertyContent("og:see_also", item);
                }
            }
            if (FacebookAdministrators != null)
            {
                foreach (string facebookAdministrator in FacebookAdministrators)
                {
                    stringBuilder.AppendMetaPropertyContentIfNotNull("fb:admins", facebookAdministrator);
                }
            }
            stringBuilder.AppendMetaPropertyContentIfNotNull("fb:app_id", FacebookApplicationId);
            stringBuilder.AppendMetaPropertyContentIfNotNull("fb:profile_id", FacebookProfileId);

            stringBuilder.AppendMetaPropertyContentIfNotNull("article:content_type", ContentType);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:category", Category);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:industry", Industry);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:published_time", PublishedTime);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:modified_time", ModifiedTime);
            stringBuilder.AppendMetaPropertyContentIfNotNull("article:expiration_time", ExpirationTime);
        }
    }
}