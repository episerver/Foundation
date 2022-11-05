using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Web;
using Foundation.Infrastructure;

namespace Foundation.Features.Blocks.ModalBlock
{
    [ContentType(DisplayName = "Modal Block",
        GUID = "CE1D5328-CC98-4383-8A75-669DCD64908E",
        Description = "Adds modal block (pop-up) to the page",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-03.png")]
    public class ModalBlock : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Show modal automatically on page load",
         Description = "Check this box to pop up the modal when page is loaded",
         Order = 2,
         GroupName = SystemTabNames.Content)]
        public virtual bool ShowModalOnPageLoad { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide modal title?",
         Description = "Check this box to hide the title section on modal",
         Order = 5,
         GroupName = SystemTabNames.Content)]
        public virtual bool HideModalTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal title",
         Order = 10)]
        public virtual string ModalTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal body",
         Order = 20,
         GroupName = SystemTabNames.Content)]
        public virtual ContentArea ModalContentArea { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Modal background image (optional)",
         Order = 25,
         GroupName = SystemTabNames.Content)]
        public virtual ContentReference ModalBackgroundImage { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide modal footer?",
         Description = "Check this box to hide footer (close button) section on modal",
         Order = 28,
         GroupName = SystemTabNames.Content)]
        public virtual bool HideModalFooter { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal close button text",
         Description = "Text to show for modal close button -- default value: 'Close'",
         Order = 30,
         GroupName = SystemTabNames.Content)]
        public virtual string ModalCloseButtonText { get; set; }
        
        [CultureSpecific]
        [Display(Name = "Show button on page to open modal?",
         Description = "Check this box to show button on page to open modal",
         Order = 40,
         GroupName = SystemTabNames.Content)]
        public virtual bool ShowModalOpenButton { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal open button text",
         Description = "Text to show for modal open button (if enabled)",
         Order = 50,
         GroupName = SystemTabNames.Content)]
        public virtual string ModalOpenButtonText { get; set; }

        [Display(Name = "CSS class",
         Description = "Custom CSS class for modal (to help with custom styles)",
         Order = 60,
         GroupName = SystemTabNames.Content)]
        public virtual string CssClass { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            HideModalTitle = false;
            HideModalFooter = true;
            ModalCloseButtonText = "Close";
            ShowModalOnPageLoad = true;
            ShowModalOpenButton = false;
            ModalOpenButtonText = "View";
        }
    }
}