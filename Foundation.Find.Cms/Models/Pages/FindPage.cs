using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Cms;
using Foundation.Cms.Pages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms.Models.Pages
{
    [ContentType(DisplayName = "Find Search Page",
        GUID = "39c8b336-bcb1-4e31-9705-9eb0fda1263b",
        Description = "Search page based on EPiServer Find",
        GroupName = CmsTabs.Content,
        AvailableInEditMode = false)]
    [AvailableContentTypes(IncludeOn = new[] { typeof(CmsHomePage) })]
    [SiteImageUrl]
    public class FindPage : FoundationPageData
    {
        [Display(
            GroupName = CmsTabs.Content,
            Order = 310)]
        [CultureSpecific]
        public virtual ContentArea RelatedContentArea { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Page Heading",
            Description = "The main heading displayed before search results.",
            GroupName = CmsTabs.Content,
            Order = 300)]
        public virtual string Heading { get; set; }


        //Allow editors to control how many hits should be displayed
        //on each search result listing when doing paging.
        [Range(0, 100)]
        [DefaultValue(10)]
        [Required]
        public virtual int PageSize { get; set; }

        //Allow editors to control wether matching keywords in 
        //search hit titles should be highlighted.
        public virtual bool HighlightTitles { get; set; }

        //Allow editors to control wether matching keywords in 
        //excerpt texts for search hits should be highlighted.
        //If false the beginning of the search text will be retrieved.
        public virtual bool HighlightExcerpts { get; set; }

        //Allow editors to specify the hight hit images should be
        //shown with. If set to 0 we don't show images. With an
        //image rescaling library we would also have had a property
        //for width but instead we let the browser scale that relative
        //to the height.
        [Range(0, 300)]
        public virtual int HitImagesHeight { get; set; }

        //Allow editors to specify how long excerpt text to retrieve
        //and show for each search hit.
        [Range(0, 1000)]
        [DefaultValue(200)]
        [Required]
        public virtual int ExcerptLength { get; set; }

        public virtual bool UseAndForMultipleSearchTerms { get; set; }
    }
}