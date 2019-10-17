using EPiServer.DataAnnotations;
using Geta.EpiCategories;

namespace Foundation.Cms.Categories
{
    [ContentType(
        GUID = "A9BBD7FC-27C5-4718-890A-E28ACBE5EE26",
        DisplayName = "Standard Category",
        Description = "Used to categorize content")]
    public class StandardCategory : CategoryData { }
}
