using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Labs.ContentManager.Dashboard;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.CallToActionBlock
{
    [ContentType(DisplayName = "Call To Action Block",
        GUID = "f82da800-c923-48f6-b701-fd093078c5d9",
        Description = "Provides a CTA anchor or link",
        GroupName = GroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-26.png")]
    public class CallToActionBlock : FoundationBlockData, IDashboardItem
    {
        #region Content
        [CultureSpecific]
        [Display(Name = "Title", Description = "Title displayed", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Subtext { get; set; }

        [Searchable(false)]
        [Display(Name = "Text color", GroupName = SystemTabNames.Content, Order = 30)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string TextColor { get; set; }
        #endregion

        #region image
        [UIHint(UIHint.Image)]
        [Display(Name = "Background image", GroupName = TabNames.Image, Order = 40)]
        public virtual ContentReference BackgroundImage { get; set; }

        [Searchable(false)]
        [SelectOne(SelectionFactoryType = typeof(BackgroundImageSelectionFactory))]
        [Display(Name = "Choose image style to fit the block", Order = 41, GroupName = TabNames.Image)]
        public virtual string BackgroundImageSetting { get; set; }
        #endregion

        [Display(GroupName = TabNames.Button, Order = 50)]
        public virtual ButtonBlock.ButtonBlock Button { get; set; }

        public void SetItem(ItemModel itemModel)
        {
            itemModel.Description = Subtext?.ToHtmlString();
            itemModel.Image = BackgroundImage;
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            TextColor = "black";
            BackgroundImageSetting = "image-default";
        }
    }
}