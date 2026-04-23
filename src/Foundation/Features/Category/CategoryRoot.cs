using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace Geta.Optimizely.Categories
{
    /// <summary>
    /// Stub for Geta.Optimizely.Categories.CategoryRoot.
    /// The Geta.Optimizely.Categories package was removed in the CMS 13 upgrade, but the
    /// database still contains a CategoryRoot content item (content link 337). This stub
    /// allows CMS to instantiate the type so Graph full sync does not fail with
    /// "Could not create instance of content type 'CategoryRoot' since it has an invalid .NET class".
    /// On startup, CMS will update tblContentType.ModelType to point to this Foundation assembly.
    /// </summary>
    [ContentType(
        GUID = "C29BF090-05BF-43EB-98D6-91575BCE4441",
        AvailableInEditMode = false,
        DisplayName = "Category Root (legacy stub)")]
    public class CategoryRoot : PageData
    {
    }
}
