using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Find.Cms.Models.Pages
{
    [ContentType(
        DisplayName = "Locations List",
        GroupName = FindTabs.Location,
        GUID = "597afd14-391b-4e99-8e4f-8827e3e82354",
        Description = "Used to display a list of all locations")]
    [AvailableContentTypes(
        Availability = Availability.Specific,
        Include = new[] { typeof(LocationItemPage) })]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-27.png")]
    public class LocationListPage : FoundationPageData
    {
        public virtual ContentArea FilterArea { get; set; }
    }
}