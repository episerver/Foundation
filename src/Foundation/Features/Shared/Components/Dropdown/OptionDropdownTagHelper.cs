using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Features.Shared.Components.Dropdown
{
    [HtmlTargetElement("dropdown-option")]
    public class OptionDropdownTagHelper : TagHelper
    {
        public bool DropdownSelected { get; set; }
        public bool DropdownChecked { get; set; }
        public bool DropdownDisabled { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";

            if (DropdownChecked)
            {
                output.Attributes.SetAttribute("checked", "checked");
            }

            if (DropdownSelected)
            {
                output.Attributes.SetAttribute("selected", "selected");
            }

            if (DropdownDisabled)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }

            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(output.Content);
        }
    }
}
