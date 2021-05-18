using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.BreadcrumbBlock
{
    [ContentType(DisplayName = "Breadcrumb Block",
        GUID = "DE43EB04-0D26-442A-91FC-E36E14A352B6",
        Description = "Render normal navigation structures as a breadcrumb",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-31.png")]
    public class BreadcrumbBlock : FoundationBlockData
    {
        [Display(Name = "Destination page", Order = 10, GroupName = SystemTabNames.Content)]
        public virtual PageReference DestinationPage { get; set; }

        [Display(Name = "Breadcrumb separator", Order = 20, GroupName = SystemTabNames.Content)]
        [SelectOne(SelectionFactoryType = typeof(BreadcrumbSeparatorSelectionFactory))]
        public virtual string Separator { get; set; }

        [Display(Name = "Alignment option", Order = 30, GroupName = SystemTabNames.Content)]
        [SelectOne(SelectionFactoryType = typeof(BreadcrumbAlignmentOptionSelectionFactory))]
        public virtual string Alignment { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Separator = "/";
            Alignment = "flex-center";
        }
    }
}
