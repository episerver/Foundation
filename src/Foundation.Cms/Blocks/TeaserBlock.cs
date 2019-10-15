using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Teaser Block", GUID = "EB67A99A-E239-41B8-9C59-20EAA5936047", GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-26.png")]
    public class TeaserBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Required(AllowEmptyStrings = false)]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Required(AllowEmptyStrings = false)]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Text { get; set; }

        [SelectOne(SelectionFactoryType = typeof(TeaserBlockTextPlacementSelectionFactory))]
        [Display(Name = "Text placement", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string TextPlacement { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Required(AllowEmptyStrings = false)]
        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual ContentReference Image { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        public virtual PageReference Link { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            TextPlacement = "AboveImage";
        }
    }
}