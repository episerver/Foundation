using Advanced.CMS.GroupingHeader;

namespace Foundation.Features.Blocks.ModalBlock
{
    [ContentType(DisplayName = "Modal Block",
        GUID = "FC6DA151-097A-442F-9583-0DD937B3E3E2",
        Description = "Adds modal block (pop-up) to the page",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-03.png")]
    public class ModalBlock : BlockData
    {
        [GroupingHeader("Modal Block Trigger Options")]
        [CultureSpecific]
        [Display(Name = "Show modal automatically on page load",
         Description = "Check this box to pop up the modal when page is loaded",
         Order = 10,
         GroupName = SystemTabNames.Content)]
        public virtual bool ShowModalOnPageLoad { get; set; }

        [CultureSpecific]
        [Display(Name = "Show modal automatically on exit intent (when mouse leaves page)",
         Description = "Check this box to pop up the modal when the visitor's mouse leaves the page",
         Order = 20,
         GroupName = SystemTabNames.Content)]
        public virtual bool ShowModalOnExitIntent { get; set; }

        [CultureSpecific]
        [Display(Name = "Show modal automatically on page scroll (% scroll depth) (0 = disabled)",
         Description = "Show modal when visitor scrolls page X % (0 = disabled)",
         Order = 30,
         GroupName = SystemTabNames.Content)]
        public virtual int ShowModalOnScrollPercentage { get; set; }

        [CultureSpecific]
        [Display(Name = "Show modal automatically after X seconds (0 = disabled)",
         Description = "Show modal after X seconds pass (0 = disabled)",
         Order = 40,
         GroupName = SystemTabNames.Content)]
        public virtual int ShowModalAfterXSeconds { get; set; }

        [CultureSpecific]
        [Display(Name = "Show button on page to open modal?",
         Description = "Check this box to show button on page to open modal",
         Order = 50,
         GroupName = SystemTabNames.Content)]
        public virtual bool ShowModalOpenButton { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal open button text",
         Description = "Text to show for modal open button (if enabled)",
         Order = 60,
         GroupName = SystemTabNames.Content)]
        public virtual string ModalOpenButtonText { get; set; }

        [CultureSpecific]
        [Display(Name = "Custom Modal open button width in px (blank = autosize)",
         Description = "Set custom width of modal open button (autosized if blank)",
         Order = 70,
         GroupName = SystemTabNames.Content)]
        public virtual int ModalOpenButtonWidth { get; set; }

        [GroupingHeader("Modal Block Header Options")]
        [CultureSpecific]
        [Display(Name = "Hide modal title?",
         Description = "Check this box to hide the title section on modal",
         Order = 80,
         GroupName = SystemTabNames.Content)]
        public virtual bool HideModalTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal title",
         Description = "Modal title",
         Order = 90,
         GroupName = SystemTabNames.Content)]
        public virtual string ModalTitle { get; set; }

        [GroupingHeader("Modal Block Body")]
        [CultureSpecific]
        [Display(Name = "Modal body",
         Order = 100,
         GroupName = SystemTabNames.Content)]
        public virtual ContentArea ModalContentArea { get; set; }

        [GroupingHeader("Modal Block Footer Options")]
        [CultureSpecific]
        [Display(Name = "Hide modal footer?",
         Description = "Check this box to hide footer (close button) section on modal",
         Order = 110,
         GroupName = SystemTabNames.Content)]
        public virtual bool HideModalFooter { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal close button text",
         Description = "Text to show for modal close button -- default value: 'Close'",
         Order = 120,
         GroupName = SystemTabNames.Content)]
        public virtual string ModalCloseButtonText { get; set; }

        [CultureSpecific]
        [Display(Name = "Modal primary button text (button hidden if blank)",
         Description = "Text to show for modal primary button -- button hidden if blank",
         Order = 130,
         GroupName = SystemTabNames.Content)]
        public virtual string ModalPrimaryButtonText { get; set; }

        [CultureSpecific]
        [Display(Name = "Primary button link (optional)",
         Description = "Link for modal primary button",
         Order = 140,
         GroupName = SystemTabNames.Content)]
        public virtual Url ModalPrimaryButtonLink { get; set; }

        [GroupingHeader("Modal Block Image Options")]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Modal background image (optional)",
         Order = 150,
         GroupName = SystemTabNames.Content)]
        public virtual ContentReference ModalBackgroundImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Modal backdrop image (covers page behind modal) (optional)",
         Order = 160,
         GroupName = SystemTabNames.Content)]
        public virtual ContentReference ModalBackdropImage { get; set; }

        [GroupingHeader("Misc Options")]
        [SelectOne(SelectionFactoryType = typeof(ModalSizeSelectionFactory))]
        [Display(Name = "Modal size", 
            Description = "Size of modal",
            Order = 165,
            GroupName = SystemTabNames.Content)]
        public virtual string ModalSize { get; set; }

        [Display(Name = "CSS class",
         Description = "Custom CSS class for modal (to help with custom styles)",
         Order = 170,
         GroupName = SystemTabNames.Content)]
        public virtual string CssClass { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            HideModalTitle = false;
            HideModalFooter = false;
            ModalCloseButtonText = "Close";
            ShowModalOnPageLoad = true;
            ShowModalOpenButton = false;
            ModalOpenButtonText = "View";
            ShowModalOnScrollPercentage = 0;
            ShowModalAfterXSeconds = 0;
            ModalSize = "";
        }
    }
}