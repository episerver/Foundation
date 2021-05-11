using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.TeaserBlock
{
    [ContentType(DisplayName = "Teaser Block",
        GUID = "EB67A99A-E239-41B8-9C59-20EAA5936047",
        Description = "Image block with overlay for text",
        GroupName = GroupNames.Content)]
    //[DefaultDisplayOption(ContentAreaTags.OneThirdWidth)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-26.png")]
    public class TeaserBlock : FoundationBlockData//, IDashboardItem
    {
        #region Content
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Description { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 21)]
        public virtual PageReference Link { get; set; }
        #endregion

        #region Header
        [CultureSpecific]
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Heading text", GroupName = TabNames.Header, Order = 10)]
        public virtual string Heading { get; set; }

        [Display(Name = "Heading size", GroupName = TabNames.Header, Order = 11)]
        public virtual int HeadingSize { get; set; }

        [SelectOne(SelectionFactoryType = typeof(TeaserBlockHeadingStyleSelectionFactory))]
        [Display(Name = "Heading style", GroupName = TabNames.Header, Order = 12)]
        public virtual string HeadingStyle { get; set; }

        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        [Display(Name = "Heading color", GroupName = TabNames.Header, Order = 13)]
        public virtual string HeadingColor
        {
            get { return this.GetPropertyValue(page => page.HeadingColor) ?? "#000000ff"; }
            set { this.SetPropertyValue(page => page.HeadingColor, value); }
        }
        #endregion

        #region Text
        [CultureSpecific]
        [Display(GroupName = TabNames.Text, Order = 30)]
        public virtual XhtmlString Text { get; set; }
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        [Display(Name = "Text color", GroupName = TabNames.Text, Order = 50)]
        public virtual string TextColor
        {
            get { return this.GetPropertyValue(page => page.TextColor) ?? "#000000ff"; }
            set { this.SetPropertyValue(page => page.TextColor, value); }
        }
        #endregion

        #region Image
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(GroupName = TabNames.Image, Order = 40)]
        public virtual ContentReference Image { get; set; }

        [Range(1, 100, ErrorMessage = "Set image width from 1 to 100")]
        [Display(Name = "Image size (%)", GroupName = TabNames.Image, Order = 41)]
        public virtual int ImageSize { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Second Image", GroupName = TabNames.Image, Order = 45)]
        public virtual ContentReference SecondImage { get; set; }

        [Range(1, 100, ErrorMessage = "Set image width from 1 to 100")]
        [Display(Name = "Image size (%)", GroupName = TabNames.Image, Order = 46)]
        public virtual int SecondImageSize { get; set; }
        #endregion

        #region Style
        [SelectOne(SelectionFactoryType = typeof(TeaserBlockHeightStyleSelectionFactory))]
        [Display(Name = "Height", GroupName = TabNames.BlockStyling, Order = 100)]
        public virtual string Height { get; set; }
        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            HeadingSize = 28;
            HeadingStyle = "none";
            HeadingColor = "#000000ff";
            ImageSize = 100;
            SecondImageSize = 100;
            BackgroundColor = "transparent";
            TextColor = "#000000ff";
        }

        //public void SetItem(ItemModel itemModel)
        //{
        //    itemModel.Description = Heading;
        //    itemModel.Image = Image;
        //}
    }
}