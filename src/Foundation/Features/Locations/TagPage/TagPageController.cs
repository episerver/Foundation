﻿using EPiServer;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Media;
using Foundation.Cms.Personalization;
using Foundation.Find.Cms.Locations.ViewModels;
using Foundation.Find.Cms.Models.Blocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Locations.TagPage
{
    public class TagPageController : PageController<Cms.Pages.TagPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICmsTrackingService _trackingService;

        public TagPageController(IContentLoader contentLoader,
            ICmsTrackingService trackingService)
        {
            _contentLoader = contentLoader;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Cms.Pages.TagPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);

            var model = new TagsViewModel(currentPage)
            {
                Continent = ControllerContext.RequestContext.GetCustomRouteData<string>("Continent")
            };

            var addcat = ControllerContext.RequestContext.GetCustomRouteData<string>("Category");
            if (addcat != null)
            {
                model.AdditionalCategories = addcat.Split(',');
            }

            var query = SearchClient.Instance.Search<Find.Cms.Models.Pages.LocationItemPage>()
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
                carousel.Items.Add(new TagsCarouselItem
                {
                    Image = location.Image,
                    Heading = location.Name,
                    Description = location.MainIntro,
                    ItemURL = location.ContentLink
                });
            }
            if (carousel.Items.All(item => item.Image == null) || currentPage.Images != null)
            {
                foreach (var image in currentPage.Images.FilteredItems.Select(ci => ci.ContentLink))
                {
                    var title = _contentLoader.Get<ImageMediaData>(image).Title;
                    carousel.Items.Add(new TagsCarouselItem { Image = image, Heading = title });
                }
            }
            model.Carousel = carousel;

            return View(model);
        }
    }
}