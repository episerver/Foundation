using EPiServer.Web;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Schema.NET;
using System;
using System.Linq;

namespace Foundation.Cms.SchemaMarkup
{
    /// <summary>
    /// Create Schema website and organization objects from CmsHomePage
    /// </summary>
    public class CmsHomePageSchemaMapper : ISchemaDataMapper<CmsHomePage>
    {
        public Thing Map(CmsHomePage content)
        {
            return new WebSite
            {
                MainEntity = new Organization
                {
                    Name = content.CompanyName,
                    Url = SiteDefinition.Current.SiteUrl,
                    ContactPoint = new ContactPoint
                    {
                        Email = content.CompanyEmail,
                        Telephone = content.CompanyPhone
                    },
                    Address = content.CompanyAddress,
                    SameAs = content.SocialLinks.Select(x => new Uri(x.Href)).ToArray()
                },
                Url = content.GetUri(true)
            };

        }
    }
}
