using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using static Foundation.Features.Shared.SelectionFactories.InclusionOrderingSelectionFactory;

namespace Foundation.Features.Sales
{
    [ContentType(DisplayName = "Sales Page",
        GUID = "9f6352bc-eea4-416a-bf76-144037c7d3db",
        Description = "Show all items on sale",
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-21.png")]
    public class SalesPage : BaseInclusionExclusionPage
    {
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ManualInclusionOrdering = InclusionOrdering.Beginning;
            PageSize = 12;
        }
    }
}