using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.LandingPages.LandingPage
{
    [ContentType(DisplayName = "Single Column Landing Page",
       GUID = "DBED4258-8213-48DB-A11F-99C034172A54",
       Description = "Default standard page that has top content area, main body, and main content area",
       GroupName = GroupNames.Content)]
    [ImageUrl("~/assets/icons/gfx/page-type-thumbnail-landingpage-onecol.png")]
    public class LandingPage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 90)]
        public virtual ContentArea TopContentArea { get; set; }
    }
}