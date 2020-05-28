using EPiServer.Core;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using Foundation.Demo.Models;
using Schema.NET;
using System;
using System.Linq;

namespace Foundation.Cms.SchemaMarkup
{
    /// <summary>
    /// Create Schema website and organization objects from DemoHomePage
    /// </summary>
    public class DemoHomePageSchemaMapper : ISchemaDataMapper<DemoHomePage>
    {
        public Thing Map(DemoHomePage content)
        {
            var potentialAction = new SearchAction { QueryInput = "required name=search_term_string" };

            if (!ContentReference.IsNullOrEmpty(content.SearchPage))
            {
                potentialAction.Target = new Uri($"{content.SearchPage.GetUri(true)}?search={{search_term_string}}");
            }

            return new WebSite
            {
                MainEntity = new Organization
                {
                    Name = content.CompanyName ?? string.Empty,
                    Url = SiteDefinition.Current.SiteUrl, //This differs from the URL below in that this should be the definitive URL for this organisation whereas the URL below may be locale-specific
                    ContactPoint = new ContactPoint
                    {
                        Email = content.CompanyEmail ?? string.Empty,
                        Telephone = content.CompanyPhone ?? string.Empty
                    },
                    Address = content.CompanyAddress ?? string.Empty,
                    SameAs = content.SocialLinks != null ? content.SocialLinks.Select(x => new Uri(x.Href ?? string.Empty)).ToArray() : new OneOrMany<Uri>()
                },
                Url = content.GetUri(true),
                PotentialAction = potentialAction
            };
        }
    }
}
