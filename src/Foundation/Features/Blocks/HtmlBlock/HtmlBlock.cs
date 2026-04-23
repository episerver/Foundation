using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Web;
using Foundation.Infrastructure;

namespace Foundation.Features.Blocks.HtmlBlock
{
    [ContentType(DisplayName = "Html block",
        GUID = "CE1D5328-CC98-4383-8A75-669DCD64908E",
        Description = "Inserts raw Html into the content",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/HtmlBlock.png")]
    public class HtmlBlock : BlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [UIHint(UIHint.Textarea)]
        public virtual string RawHtml { get; set; }
    }

    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = UiHint)]
    public class TallWideTextboxEditorDescriptor : EditorDescriptor
    {
        public const string UiHint = "TallWideTextbox";

        public TallWideTextboxEditorDescriptor()
        {
            ClientEditingClass = "epi/shell/widget/ValidationTextarea";
            Style = "width:600px;";
        }

        protected override void SetEditorConfiguration(ExtendedMetadata metadata)
        {
            EditorConfiguration["style"] = Style;
            base.SetEditorConfiguration(metadata);
        }

        // Properties
        public string Style { get; set; }
    }
}