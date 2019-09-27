using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(GUID = "426CF12F-1F01-4EA0-922F-0778314DDAF0",
        DisplayName = "Button Block",
        GroupName = CmsTabs.Content,
        Description = "Used to insert a link which is styled as a button",
        AvailableInEditMode = true)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-26.png")]
    public class ButtonBlock : FoundationBlockData
    {
        [Display(Order = 1, Name = "Text", GroupName = SystemTabNames.Content)]
        public virtual string ButtonText { get; set; }

        [Display(Order = 2, Name = "Link", GroupName = SystemTabNames.Content)]
        public virtual Url ButtonLink { get; set; }

        [Display(Order = 3, Name = "Button style", GroupName = SystemTabNames.Content)]
        [SelectOne(SelectionFactoryType = typeof(ButtonBlockStyleSelectionFactory))]
        public virtual string ButtonStyle { get; set; }

        [Display(Order = 4, Name = "Reassuring caption", GroupName = SystemTabNames.Content, Prompt = "Cancel anytime...")]
        public virtual string ButtonCaption { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ButtonStyle = "button-transparent-black";
        }
    }
}