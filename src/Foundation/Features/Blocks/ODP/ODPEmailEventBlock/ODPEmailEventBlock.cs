using EPiServer;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Blocks.ODP.ODPEmailEventBlock
{
    [ContentType(DisplayName = "ODP Email Event Block",
        GUID = "f1f3766e-bd5b-4294-97f2-cd54c8cf0f9d",
        Description = "Set an email event.",
        GroupName = GroupNamesCustom.Odp)]
    [ImageUrl("/icons/cms/blocks/HtmlBlock.png")]

    public class ODPEmailEventBlock : BaseODPEventBlock
    {
        [CultureSpecific]
        [Required]
        [Display(Name = "Action", Order = 10, GroupName = SystemTabNames.Content)]
        [SelectOne(SelectionFactoryType = typeof(EmailActionEventFactory))]
        public virtual string Action { get; set; }

        [CultureSpecific]
        [Display(Name = "Campaign", Order = 20, GroupName = SystemTabNames.Content)]
        public virtual string Campaign { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ShowSnippetEditMode = false;
            ShowSnippetFrontEnd = false;
        }
    }

    public class EmailActionEventFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Sent", Value = "sent" },
                new SelectItem { Text = "Open", Value = "open" },
                new SelectItem { Text = "Click", Value = "click" }                
            };
        }
    }

    public class ODPEmailEventBlockComponent : AsyncBlockComponent<ODPEmailEventBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(ODPEmailEventBlock currentBlock)
        {
            return await Task.FromResult(View("~/Features/Blocks/ODP/ODPEmailEventBlock/ODPEmailEventBlock.cshtml", currentBlock));
        }
    }
}
