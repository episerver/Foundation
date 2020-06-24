using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Locations.TagPage
{
    public class TagsViewModel : ContentViewModel<TagPage>
    {
        public TagsViewModel(TagPage currentPage) : base(currentPage)
        {
        }

        public string Continent { get; set; }

        public string[] AdditionalCategories { get; set; }

        public TagsCarouselViewModel Carousel { get; set; }

        public List<LocationItemPage.LocationItemPage> Locations { get; set; }
    }
}