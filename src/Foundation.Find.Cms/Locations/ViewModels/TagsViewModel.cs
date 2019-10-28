using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;
using Foundation.Find.Cms.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Find.Cms.Locations.ViewModels
{
    public class TagsViewModel : ContentViewModel<TagPage>
    {
        public TagsViewModel(TagPage currentPage) : base(currentPage)
        {
        }

        public string Continent { get; set; }

        public string[] AdditionalCategories { get; set; }

        public TagsCarouselViewModel Carousel { get; set; }

        public List<LocationItemPage> Locations { get; set; }
    }
}