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
        Description = "Allows for creation of rich standard pages",
        GroupName = SystemTabNames.Content)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-23.png")]
    public class StandardPage : FoundationPageData
    {
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Background image", GroupName = SystemTabNames.Content, Order = 205)]
        public virtual ContentReference BackgroundImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
        [Display(Name = "Background video", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentReference BackgroundVideo { get; set; }

        [SelectOne(SelectionFactoryType = typeof(FoundationStandardPageTopPaddingModeSelectionFactory))]
        [Display(Name = "Top padding mode",
            Description = "Sets how much padding should be at the top of the standard content",
            GroupName = SystemTabNames.Content,
            Order = 220)]
        public virtual string TopPaddingMode { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            TopPaddingMode = FoundationStandardPageTopPaddingModeSelectionFactory.FoundationStandardPageTopPaddingModes.None;
        }
    }
}