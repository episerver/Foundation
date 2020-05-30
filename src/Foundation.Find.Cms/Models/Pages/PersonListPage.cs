using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Find.Cms.Models.Pages
{
    [ContentType(DisplayName = "Person List Page", 
        GUID = "4f0203b6-d49e-4683-9ce6-ede8c37c77d3", 
        Description = "Used to find people within an organisation",
        GroupName = FindTabNames.Person)]
    [AvailableContentTypes(Availability.Specific, Include = new[] { typeof(PersonListPage), typeof(PersonItemPage) })]
    [ImageUrl("~/assets/icons/cms/pages/contactcatalogue.png")]
    public class PersonListPage : FoundationPageData
    {
    }
}