using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Standard Page",
        GUID = "c0a25bb7-199c-457d-98c6-b0179c7acae8",
        GroupName = CmsTabs.Content,
        Description = "Allows for creation of rich standard pages.")]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-23.png")]
    public class StandardPage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Main Background Video", GroupName = SystemTabNames.Content)]
        [UIHint(UIHint.Video)]
        public virtual ContentReference MainBackgroundVideo { get; set; }

        [Display(Name = "Top Padding Mode", Description = "Sets how much padding should be at the top of the standard content", GroupName = SystemTabNames.Content)]
        [SelectOne(SelectionFactoryType = typeof(FoundationStandardPageTopPaddingModeSelectionFactory))]
        public virtual string TopPaddingMode { get; set; }

        [CultureSpecific]
        [Display(Name = "Main Content Items", GroupName = SystemTabNames.Content)]
        public virtual ContentArea MainContentItems { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            TopPaddingMode = FoundationStandardPageTopPaddingModeSelectionFactory.FoundationStandardPageTopPaddingModes.None;
        }
    }
}