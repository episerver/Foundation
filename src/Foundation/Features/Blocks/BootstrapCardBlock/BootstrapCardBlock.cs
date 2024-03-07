using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Advanced.CMS.GroupingHeader;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Labs.ContentManager.Dashboard;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using Foundation.Infrastructure.Cms.Attributes;

namespace Foundation.Features.Blocks.BootstrapCardBlock
{
    [ContentType(DisplayName = "Bootstrap Card Block",
        GUID = "A0D9E5C6-3030-4D9C-9FCE-F51D1ABB2A04",
        Description = "Adds bootstrap card block to the page",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-03.png")]
    public class BootstrapCardBlock : FoundationBlockData, IDashboardItem
    {
        [GroupingHeader("Card Alignment Options")]
        // Card alignment -- left/center/right
        [SelectOne(SelectionFactoryType = typeof(CardAlignmentSelectionFactory))]
        [Display(Name = "Card alignment",
         Order = 5,
         GroupName = SystemTabNames.Content)]
        public virtual string CardAlignment { get; set; }

        [GroupingHeader("Card Text and Content Properties")]
        [CultureSpecific]
        [Display(Name = "Card header",
         Order = 10)]
        public virtual string CardHeader { get; set; }
        
        [CultureSpecific]
        [Display(Name = "Card title",
         Order = 20)]
        public virtual string CardTitle { get; set; }

        // Card title size -- h5 (default), h4, h3, h2, h1
        [SelectOne(SelectionFactoryType = typeof(TitleSizeSelectionFactory))]
        [CultureSpecific]
        [Display(Name = "Card title size",
         Order = 23)]
        public virtual string CardTitleSize { get; set; }

        [CultureSpecific]
        [Display(Name = "Card subtitle",
         Order = 30)]
        public virtual string CardSubtitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Card body",
         Order = 40,
         GroupName = SystemTabNames.Content)]
        [AllowedTypes(new[] { typeof(IContentData) })]
        public virtual ContentArea CardContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Card footer",
         Order = 60)]
        public virtual string CardFooter { get; set; }

        [GroupingHeader("Card Image Properties")]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Card image (optional)",
         Order = 80,
         GroupName = SystemTabNames.Content)]
        public virtual ContentReference CardImage { get; set; }

        [GroupingHeader("Card Button and Link Properties")]
        [CultureSpecific]
        [Display(Name = "Card button text (button hidden if blank)",
         Description = "Text to show for card button -- button hidden if blank",
         Order = 100,
         GroupName = SystemTabNames.Content)]
        public virtual string CardButtonText { get; set; }

        [CultureSpecific]
        [Display(Name = "Card button link (optional)",
         Description = "Link for card button",
         Order = 120,
         GroupName = SystemTabNames.Content)]
        public virtual Url CardButtonLink { get; set; }

        [Display(Name = "Make entire card clickable?",
         Description = "Click anywhere on card to browse to URL defined in button (instead of only clicking the button)?",
         Order = 125,
         GroupName = SystemTabNames.Content)]
        public virtual bool CardClickable { get; set; }

        [CultureSpecific]
        //[UIHint("FooterColumnNavigation")]
        [Display(Name = "Card Links (0-3 links supported)",
         Description = "Links to show on card (0-3 links supported)",
         Order = 140,
         GroupName = SystemTabNames.Content)]
        [MaxElements(3)]
        public virtual LinkItemCollection CardLinks { get; set; }

        [GroupingHeader("Misc Properties")]
        [Display(Name = "CSS class",
         Description = "Custom CSS class for card (to help with custom styles)",
         Order = 200,
         GroupName = SystemTabNames.Content)]
        public virtual string CssClass { get; set; }

        public void SetItem(ItemModel itemModel)
        {
            itemModel.Description = CardTitle;
            itemModel.Image = CardImage;
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            CardAlignment = "";
            CardTitleSize = "h5";
            CardButtonText = "";
        }

        public class CardAlignmentSelectionFactory : ISelectionFactory
        {
            public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
            {
                return new ISelectItem[]
                {
                    new SelectItem { Text = "Left (default)", Value = "" },
                    new SelectItem { Text = "Center", Value = "text-center" },
                    new SelectItem { Text = "Right	", Value = "text-end" },
                };
            }
        }

        public class TitleSizeSelectionFactory : ISelectionFactory
        {
            public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
            {
                return new ISelectItem[]
                {
                    new SelectItem { Text = "h5 (default)", Value = "h5" },
                    new SelectItem { Text = "h4", Value = "h4" },
                    new SelectItem { Text = "h3", Value = "h3" },
                    new SelectItem { Text = "h2", Value = "h2" },
                    new SelectItem { Text = "h1", Value = "h1" },
                };
            }
        }
    }
}