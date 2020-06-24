using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.ButtonBlock
{
    [ContentType(DisplayName = "Button Block",
        GUID = "426CF12F-1F01-4EA0-922F-0778314DDAF0",
        Description = "Used to insert a link which is styled as a button",
        GroupName = GroupNames.Content,
        AvailableInEditMode = true)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-26.png")]
    public class ButtonBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(Name = "Label", Order = 10, GroupName = SystemTabNames.Content)]
        public virtual string ButtonText { get; set; }

        [Display(Name = "Link", Order = 20, GroupName = SystemTabNames.Content)]
        public virtual Url ButtonLink { get; set; }

        [SelectOne(SelectionFactoryType = typeof(ButtonBlockStyleSelectionFactory))]
        [Display(Name = "Style", Order = 30, GroupName = SystemTabNames.Content)]
        public virtual string ButtonStyle { get; set; }

        [CultureSpecific]
        [Display(Name = "Reassuring caption", Order = 40, GroupName = SystemTabNames.Content, Prompt = "Cancel anytime...")]
        public virtual string ButtonCaption { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ButtonStyle = "button-transparent-black";
        }
    }
}