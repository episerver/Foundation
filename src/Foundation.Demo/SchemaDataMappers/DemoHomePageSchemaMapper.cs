using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Demo.Models;
using Schema.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Cms.SchemaMarkup
{
    /// <summary>
    /// Create Schema website and organization objects from DemoHomePage
    /// </summary>
    public class DemoHomePageSchemaMapper : ISchemaDataMapper<DemoHomePage>
    {
        public Thing Map(DemoHomePage content)
        {
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
                    SameAs = content.SocialLinks.Select(x => new Uri(x.Href ?? string.Empty)).ToArray()
                },
                Url = content.GetUri(true),
                PotentialAction = new SearchAction
                {
                    Target = new Uri($"{content.SearchPage.GetUri(true)}?search={{search_term_string}}"),
                    QueryInput = "required name=search_term_string"
                }
            };
        }
    }
}
