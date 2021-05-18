using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Mvc;
using Foundation.Features.Media;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Locations.TagPage
{
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

            var query = SearchClient.Instance.Search<LocationItemPage.LocationItemPage>()
                .Filter(f => f.TagString().Match(currentPage.Name));
            if (model.AdditionalCategories != null)
            {
                query = model.AdditionalCategories.Aggregate(query, (current, c) => current.Filter(f => f.TagString().Match(c)));
            }
            if (model.Continent != null)
            {
                query = query.Filter(dp => dp.Continent.MatchCaseInsensitive(model.Continent));
            }
            model.Locations = query.StaticallyCacheFor(new System.TimeSpan(0, 1, 0)).GetContentResult().ToList();

            //Add theme images from results
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
                if (currentPage.Images != null && currentPage.Images.FilteredItems != null)
                {
                    foreach (var image in currentPage.Images.FilteredItems.Select(ci => ci.ContentLink))
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