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

namespace Foundation.Features.Blocks.ODP.ODPSearchEventBlock
{
    [ContentType(DisplayName = "ODP Search Event Block",
        GUID = "77148120-1964-40d0-ac09-48b305649b35",
        Description = "Set a search event.",
        GroupName = GroupNamesCustom.Odp)]
    [ImageUrl("/icons/cms/blocks/HtmlBlock.png")]

    public class ODPSearchEventBlock : BaseODPEventBlock
    {
        [CultureSpecific]
        [Required]
        [Display(Name = "Search Term", Order = 10, GroupName = SystemTabNames.Content)]
        public virtual string SearchTerm { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ShowSnippetEditMode = false;
            ShowSnippetFrontEnd = false;
        }
    }

    public class ODPSearchEventBlockComponent : AsyncBlockComponent<ODPSearchEventBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(ODPSearchEventBlock currentBlock)
        {
            return await Task.FromResult(View("~/Features/Blocks/ODP/ODPSearchEventBlock/ODPSearchEventBlock.cshtml", currentBlock));
        }
    }
}
