using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Roles Page", 
        GUID = "9c1a7a97-b619-4e13-9072-14ce1f3fe178", 
        Description = "Manage B2B roles")]
    public class RolesPage : FoundationPageData
    {
    }
}