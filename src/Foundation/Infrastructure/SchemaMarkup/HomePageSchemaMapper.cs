using EPiServer.Core;
using EPiServer.Web;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Features.Home;
using Foundation.Features.Settings;
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
        private readonly ISettingsService _settingsService;

        public HomePageSchemaMapper(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public Thing Map(HomePage content)
        {
            var referencePages = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();
            var potentialAction = new SearchAction { QueryInput = "required name=search_term_string" };

            if (!ContentReference.IsNullOrEmpty(referencePages?.SearchPage))
            {
                potentialAction.Target = new Uri($"{referencePages.SearchPage.GetUri(true)}?search={{search_term_string}}");
            }

            return new WebSite
            {
                MainEntity = new Organization
                {
                    Name = layoutSettings?.CompanyName ?? string.Empty,
                    Url = SiteDefinition.Current.SiteUrl,
                    ContactPoint = new ContactPoint
                    {
                        Email = layoutSettings?.CompanyEmail ?? string.Empty,
                        Telephone = layoutSettings?.CompanyPhone ?? string.Empty
                    },
                    Address = layoutSettings?.CompanyAddress ?? string.Empty,
                    SameAs = layoutSettings?.SocialLinks?.Select(x => new Uri(x.Href)).ToArray() ?? new OneOrMany<Uri>()
                },
                Url = content.GetUri(true),
                PotentialAction = potentialAction
            };
        }
    }
}
