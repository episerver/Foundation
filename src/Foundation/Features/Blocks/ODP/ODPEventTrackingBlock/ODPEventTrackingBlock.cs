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

namespace Foundation.Features.Blocks.ODP.ODPEventTrackingBlock
{
    [ContentType(DisplayName = "ODP Event Tracking Block",
        GUID = "311ba8a3-f0cd-471c-a9cc-deb6432356bf",
        Description = "Set a tracking event on the page.",
        GroupName = GroupNamesCustom.Odp)]
    [ImageUrl("/icons/cms/blocks/HtmlBlock.png")]
    public class ODPEventTrackingBlock : BaseODPEventBlock
    {
  
        [Display(Name = "Event Type", Order = 10, GroupName = SystemTabNames.Content)]
        [SelectOne(SelectionFactoryType = typeof(EventTypeSelectionFactory))]
        public virtual string EventType { get; set; }

        [CultureSpecific]
        [Display(Name = "Other Event Type (Set if using an event not in the list) ",
            Description = "Set event field that is not on the list",
            ShortName = "This will be used if set.",
            Order = 20, GroupName = SystemTabNames.Content)]
        public virtual string Other { get; set; }

        [CultureSpecific]
        [Display(Name = "Event Action", Order = 30, GroupName = SystemTabNames.Content)]
        public virtual string EventAction { get; set; }

        [CultureSpecific]
        [Display(Name = "Campaign (Displays in the profile events view in ODP)",
            Description = "Displays in the profile events view in ODP",
            Order = 50,
            GroupName = SystemTabNames.Content)]
        public virtual string Campaign { get; set; }

        [CultureSpecific]
        [Display(Name = "Email (If set, will associate journey with the user)", Order = 60, GroupName = SystemTabNames.Content)]
        public virtual string Email { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ShowSnippetEditMode = false;
            ShowSnippetFrontEnd = false;
        }
    }

    public class EventTypeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Consent", Value = "consent" },
                new SelectItem { Text = "List", Value = "list" },
                new SelectItem { Text = "Order", Value = "order" },
                new SelectItem { Text = "Page View", Value = "pageview" },
                new SelectItem { Text = "Product", Value = "product" },
                new SelectItem { Text = "Push", Value = "push" },
                new SelectItem { Text = "Reachability", Value = "reachability" },
                new SelectItem { Text = "Web Form", Value = "web_form" }
            };
        }
    }

    public class ODPEventTrackingBlockComponent : AsyncBlockComponent<ODPEventTrackingBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(ODPEventTrackingBlock currentBlock)
        {
            return await Task.FromResult(View("~/Features/Blocks/ODP/ODPEventTrackingBlock/ODPEventTrackingBlock.cshtml", currentBlock));
        }
    }
}