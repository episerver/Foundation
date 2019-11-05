using EPiServer.Core;
using EPiServer.DataAbstraction;
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
        GroupName = SystemTabNames.Content,
        AvailableInEditMode = false)]
    [AvailableContentTypes(IncludeOn = new[] { typeof(CmsHomePage) })]
    [SiteImageUrl]
    public class FindPage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Page heading",
           Description = "The main heading displayed before search results.",
           GroupName = SystemTabNames.Content,
           Order = 10)]
        public virtual string Heading { get; set; }
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 310)]
        public virtual ContentArea RelatedContentArea { get; set; }

        //Allow editors to control how many hits should be displayed
        //on each search result listing when doing paging.
        [Required]
        [Range(0, 100)]
        [DefaultValue(10)]
        [Display(Name = "Page size", GroupName = SystemTabNames.Content, Order = 320)]
        public virtual int PageSize { get; set; }

        //Allow editors to control wether matching keywords in 
        //search hit titles should be highlighted.
        [Display(Name = "Hightlight titles", GroupName = SystemTabNames.Content, Order = 330)]
        public virtual bool HighlightTitles { get; set; }

        //Allow editors to control wether matching keywords in 
        //excerpt texts for search hits should be highlighted.
        //If false the beginning of the search text will be retrieved.
        [Display(Name = "Hightlight excerpts", GroupName = SystemTabNames.Content, Order = 340)]
        public virtual bool HighlightExcerpts { get; set; }

        //Allow editors to specify the hight hit images should be
        //shown with. If set to 0 we don't show images. With an
        //image rescaling library we would also have had a property
        //for width but instead we let the browser scale that relative
        //to the height.
        [Range(0, 300)]
        [Display(Name = "Hit images height", GroupName = SystemTabNames.Content, Order = 350)]
        public virtual int HitImagesHeight { get; set; }

        //Allow editors to specify how long excerpt text to retrieve
        //and show for each search hit.
        [Required]
        [Range(0, 1000)]
        [DefaultValue(200)]
        [Display(Name = "Excerpt lenght", GroupName = SystemTabNames.Content, Order = 360)]
        public virtual int ExcerptLength { get; set; }

        [Display(Name = "Use and for multiple search terms", GroupName = SystemTabNames.Content, Order = 370)]
        public virtual bool UseAndForMultipleSearchTerms { get; set; }
    }
}