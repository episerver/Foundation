using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System;


namespace Foundation.Features.Blocks.JourneyContainerBlock
{
    [ContentType(DisplayName = "Journey Container Block",
        GUID = "881458bd-9a30-4b13-9840-b118e4b72345",
        Description = "",
        GroupName = GroupNamesCustom.Odp)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-04.png")]
    public class JourneyContainerBlock : FoundationBlockData
    {
        [Required]
        [Display(Name = "Journey Start Time",
           Description = "Start for Journey. All ODP blocks will be offset by 2 minutes each.",
           Order = 10)]
        public virtual DateTime JourneyStartTime { get; set; }

        [Display(Name = "Main content area",
            Order = 30)]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(Name = "CSS class",
            Order = 20)]
        public virtual string CssClass { get; set; }

    }
}