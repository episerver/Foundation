using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Locations.TagPage
{
    [ContentType(DisplayName = "Tags Page",
        GUID = "fc83ded1-be4a-40fe-99b2-9ab739b018d5",
        Description = "Used to define a Tag",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/pages/cms-icon-page-27.png")]
    public class TagPage : FoundationPageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [AllowedTypes(typeof(ImageData))]
        public virtual ContentArea Images { get; set; }

        [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual ContentArea TopContentArea { get; set; }

        [StringLength(5000)]
        [Display(Name = "Intro text", GroupName = SystemTabNames.Content, Order = 95)]
        public virtual string MainIntro { get; set; }

        [Display(Name = "Bottom content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea BottomArea { get; set; }

        [Ignore]
        public string SearchSection => "Tags";

        [Ignore]
        public string SearchHitType => "Tag";
    }
}