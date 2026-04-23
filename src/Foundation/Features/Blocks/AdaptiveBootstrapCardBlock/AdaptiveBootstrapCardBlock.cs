using System.ComponentModel.DataAnnotations;
using AdaptiveImages.Core;
using AdaptiveImages.Models;
using AdaptiveImages.Validation;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.Blocks.AdaptiveBootstrapCardBlock
{
    [ContentType(DisplayName = "Adaptive Bootstrap Multi-Image Card Block",
        GUID = "89FC1AA1-C5F1-4CA5-9B9F-ED2540D04AE5",
        Description = "Adds bootstrap card block to the page",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-03.png")]
    public class AdaptiveBootstrapCardBlock : FoundationBlockData
    {
        // [CultureSpecific]
        // [Display(Name = "Hide modal title?",
        //  Description = "Check this box to hide the title section on modal",
        //  Order = 5,
        //  GroupName = SystemTabNames.Content)]
        // public virtual bool HideModalTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Card header",
         Order = 10)]
        public virtual string CardHeader { get; set; }
        
        [CultureSpecific]
        [Display(Name = "Card title",
         Order = 20)]
        public virtual string CardTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Card subtitle",
         Order = 30)]
        public virtual string CardSubtitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Card body",
         Order = 40,
         GroupName = SystemTabNames.Content)]
        public virtual ContentArea CardContentArea { get; set; }
        
        [CultureSpecific]
        [Display(Name = "Card footer",
         Order = 50)]
        public virtual string CardFooter { get; set; }

        [CultureSpecificImage]
        [Display(Name = "Card image (optional)",
         Order = 60,
         GroupName = SystemTabNames.Content)]
        [Proportions(AllowCropping = true, AllowFocalPoint = true)]
        public virtual SingleImage CardImage { get; set; }
        
        [CultureSpecificImage]
        [Display(Name = "Card image (adaptive) (optional)",
         Order = 80,
         GroupName = SystemTabNames.Content)]
        //[Proportions(16, 9, FormFactor.Large, AllowCropping = true, AllowFocalPoint = true)]
        //[Proportions(AllowCropping = true, AllowFocalPoint = true)]
        // Widescreen image at least 1920x1080 for desktop
        [Size(1280, FormFactor.Large)]
        [Proportions(3, 2, FormFactor.Large, "Landscape")]
        [Proportions(16, 9, FormFactor.Large, "Widescreen", IsDefault = true, AllowCropping = true, AllowFocalPoint = true)]
        // Portrait proportions at least 1280x960 for tablet
        [Size(768, FormFactor.Medium)]
        [Proportions(4, 3, FormFactor.Medium, AllowCropping = true, AllowFocalPoint = true)]
        // Square image at least 768x768 for mobile
        [Size(540, FormFactor.Small)]
        [Proportions(1, 1, FormFactor.Small, AllowCropping = true, AllowFocalPoint = true)]
        public virtual AdaptiveImage CardAdaptiveImage { get; set; }
        
        //[CultureSpecific]
        //[UIHint(UIHint.Image)]
        //[Display(Name = "Card image (optional)",
        // Order = 60,
        // GroupName = SystemTabNames.Content)]
        //public virtual ContentReference CardImage { get; set; }

        // Card alignment -- left/center/right

        // Card links -- add up to three? Or have link1, link2, link3?

        // Card list group


        [Display(Name = "CSS class",
         Description = "Custom CSS class for card (to help with custom styles)",
         Order = 100,
         GroupName = SystemTabNames.Content)]
        public virtual string CssClass { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            // HideModalTitle = false;
            // HideModalFooter = false;
            // ModalCloseButtonText = "Close";
            // ShowModalOnPageLoad = true;
            // ShowModalOpenButton = false;
            // ModalOpenButtonText = "View";
        }
    }
}