using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace Foundation.Features.Category
{
    /// <summary>
    /// Stub for Foundation.Features.Category.StandardCategory.
    /// This type was removed from the Foundation codebase but the database still references it
    /// (content link 338, child of CategoryRoot 337). This stub allows CMS to instantiate it
    /// so Graph full sync can traverse the content tree without errors.
    /// </summary>
    [ContentType(
        GUID = "A9BBD7FC-27C5-4718-890A-E28ACBE5EE26",
        AvailableInEditMode = false,
        DisplayName = "Standard Category (legacy stub)")]
    public class StandardCategory : PageData
    {
    }
}
