using Foundation.Features.Media;

namespace Foundation.Features.Locations.TagPage
{
    // EPiServer.Find removed: location queries replaced with IContentLoader.
    // TagString/continent/category filtering is not available without Find;
    // all locations are returned unfiltered.
    public class TagPageController : PageController<TagPage>
    {
        private readonly IContentLoader _contentLoader;

        public TagPageController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public ActionResult Index(TagPage currentPage)
        {
            var model = new TagsViewModel(currentPage)
            {
                Continent = RouteData.Values["Continent"]?.ToString()
            };

            var addcat = RouteData.Values["Category"]?.ToString();
            if (addcat != null)
            {
                model.AdditionalCategories = addcat.Split(',');
            }

            // EPiServer.Find removed: load locations from parent page via IContentLoader.
            // Tag/continent filtering requires Find and is not available.
            var parent = _contentLoader.Get<IContent>(currentPage.ParentLink);
            if (parent is LocationListPage.LocationListPage listPage)
            {
                model.Locations = _contentLoader
                    .GetChildren<LocationItemPage.LocationItemPage>(listPage.ContentLink)
                    .ToList();
            }
            else
            {
                model.Locations = new List<LocationItemPage.LocationItemPage>();
            }

            var carousel = new TagsCarouselViewModel
            {
                Items = new List<TagsCarouselItem>()
            };

            foreach (var location in model.Locations)
            {
                if (location.Image != null)
                {
                    carousel.Items.Add(new TagsCarouselItem
                    {
                        Image = location.Image,
                        Heading = location.Name,
                        Description = location.MainIntro,
                        ItemURL = location.ContentLink
                    });
                }
            }

            if (carousel.Items.All(item => item.Image == null) || currentPage.Images != null)
            {
                if (currentPage.Images?.Items != null)
                {
                    foreach (var image in currentPage.Images.Items.Select(ci => ci.ContentLink))
                    {
                        var title = _contentLoader.Get<ImageMediaData>(image).Title;
                        carousel.Items.Add(new TagsCarouselItem { Image = image, Heading = title });
                    }
                }
            }

            model.Carousel = carousel;
            return View(model);
        }
    }
}
