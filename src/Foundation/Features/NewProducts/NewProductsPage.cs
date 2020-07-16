using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using static Foundation.Features.Shared.SelectionFactories.InclusionOrderingSelectionFactory;

namespace Foundation.Features.NewProducts
{
    [ContentType(DisplayName = "New Products Page",
        GUID = "3ce903a3-3d48-4fe3-92f5-14b5e6f393b5",
        Description = "Show the top new products by sorted by the creation date",
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-21.png")]
    public class NewProductsPage : BaseInclusionExclusionPage
    {
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ManualInclusionOrdering = InclusionOrdering.Beginning;
            NumberOfProducts = 12;
            PageSize = 12;
        }
    }
}