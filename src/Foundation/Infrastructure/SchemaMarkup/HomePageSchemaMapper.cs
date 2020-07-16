using EPiServer.Core;
using EPiServer.Web;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Features.Home;
using Schema.NET;
using System;
using System.Linq;

namespace Foundation.Infrastructure.SchemaMarkup
{
    /// <summary>
    /// Create Schema website and organization objects from CmsHomePage
    /// </summary>
    public class HomePageSchemaMapper : ISchemaDataMapper<HomePage>
    {
        public Thing Map(HomePage content)
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
                    Url = SiteDefinition.Current.SiteUrl,
                    ContactPoint = new ContactPoint
                    {
                        Email = content.CompanyEmail ?? string.Empty,
                        Telephone = content.CompanyPhone ?? string.Empty
                    },
                    Address = content.CompanyAddress ?? string.Empty,
                    SameAs = content.SocialLinks.Select(x => new Uri(x.Href)).ToArray()
                },
                Url = content.GetUri(true),
                PotentialAction = potentialAction
            };

        }
    }
}
