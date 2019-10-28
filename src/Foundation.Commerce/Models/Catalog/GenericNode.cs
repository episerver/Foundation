using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Catalog
{
    [CatalogContentType(DisplayName = "Generic Node", GUID = "4ac27ad4-bf60-4ea0-9a77-28a89d38d3fd", Description = "")]
    public class GenericNode : NodeContent, IProductRecommendations
    {
        [CultureSpecific]
        [Display(Name = "Long name", GroupName = SystemTabNames.Content, Order = 5)]
        [BackingType(typeof(PropertyString))]
        public virtual string LongName { get; set; }

        [CultureSpecific]
        [Display(Name = "Teaser", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Teaser { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", GroupName = SystemTabNames.Content, Order = 15)]
        public virtual XhtmlString Description { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Top content area",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual ContentArea TopContentArea { get; set; }

        [Display(Name = "Partial page size", Order = 25)]
        public virtual int PartialPageSize { get; set; }

        [CultureSpecific]
        [Display(Name = "Show recommendations", Description = "This will determine whether or not to show recommendations.", Order = 30)]
        public virtual bool ShowRecommendations { get; set; }

        public override void SetDefaultValues(ContentType contentType) => ShowRecommendations = true;
    }
}