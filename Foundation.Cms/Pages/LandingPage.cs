using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(
       DisplayName = "Standard Single Column Landing Page",
       Description = "Defauult standard page that has top content area, main body, and main content area.",
       GUID = "DBED4258-8213-48DB-A11F-99C034172A54",
       GroupName = CmsTabs.Content)]
    [ImageUrl("~/assets/icons/gfx/page-type-thumbnail-landingpage-onecol.png")]
    public class LandingPage : FoundationPageData
    {
        [Display(
           GroupName = SystemTabNames.Content,
           Order = 330)]
        public virtual ContentArea TopContentArea { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            HideSiteFooter = true;
            HideSiteHeader = true;
        }
    }
}
