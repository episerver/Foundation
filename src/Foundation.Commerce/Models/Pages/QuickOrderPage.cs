using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;
using Foundation.Commerce.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Quick Order Page",
        GUID = "9F846F7D-2DFA-4983-815D-C09B12CEF993",
        AvailableInEditMode = false,
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-14.png")]
    public class QuickOrderPage : FoundationPageData, IDisableOPE
    {
        [CultureSpecific]
        [Display(Name = "Quick Order Block content area", GroupName = SystemTabNames.Content, Order = 20)]
        [AllowedTypes(typeof(QuickOrderBlock))]
        public virtual ContentArea QuickOrderBlockContentArea { get; set; }
    }
}