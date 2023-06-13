using Foundation.Features.Shared.EditorDescriptors;

namespace Foundation.Features.MyOrganization.QuickOrderPage
{
    [ContentType(DisplayName = "Quick Order Page",
        GUID = "9F846F7D-2DFA-4983-815D-C09B12CEF993",
        AvailableInEditMode = false,
        GroupName = GroupNames.Commerce)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-14.png")]
    public class QuickOrderPage : FoundationPageData, IDisableOPE
    {
        [CultureSpecific]
        [Display(Name = "Quick Order Block content area", GroupName = SystemTabNames.Content, Order = 20)]
        [AllowedTypes(typeof(QuickOrderBlock.QuickOrderBlock))]
        public virtual ContentArea QuickOrderBlockContentArea { get; set; }
    }
}