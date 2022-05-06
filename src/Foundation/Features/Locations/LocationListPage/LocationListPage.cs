using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Locations.Blocks;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Locations.LocationListPage
{
    [ContentType(DisplayName = "Locations List Page",
        GUID = "597afd14-391b-4e99-8e4f-8827e3e82354",
        Description = "Used to display a list of all locations",
        GroupName = TabNames.Location)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-27.png")]
    [AvailableContentTypes(Availability = Availability.Specific, Include = new[] { typeof(LocationItemPage.LocationItemPage) })]
    public class LocationListPage : FoundationPageData
    {
        [AllowedTypes(new[] { typeof(FilterActivitiesBlock), typeof(FilterContinentsBlock), typeof(FilterDistancesBlock), typeof(FilterTemperaturesBlock) })]
        [Display(Name = "Filter area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea FilterArea { get; set; }
    }
}