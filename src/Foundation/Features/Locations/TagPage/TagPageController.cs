using EPiServer;
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
                model.AdditionalCategories = addcat.Split(',');

            var carousel = new CarouselViewModel
            {
                Items = new List<CarouselItemBlock>()
            };


            if (currentPage.Images != null)
            {
                foreach (var img in currentPage.Images.FilteredItems.Select(ci => ci.ContentLink))
                {
                    var t = _contentLoader.Get<ImageMediaData>(img).Title;
                    carousel.Items.Add(new CarouselItemBlock { Image = img, Heading = t });
                }
            }
            var q = SearchClient.Instance.Search<Find.Cms.Models.Pages.LocationItemPage>()
                .Filter(f => f.TagString().Match(currentPage.Name));

            if (model.AdditionalCategories != null)
            {
                q = model.AdditionalCategories.Aggregate(q, (current, c) => current.Filter(f => f.TagString().Match(c)));
            }
            if (model.Continent != null)
            {
                q = q.Filter(dp => dp.Continent.MatchCaseInsensitive(model.Continent));
            }
            var res = q.StaticallyCacheFor(new System.TimeSpan(0, 1, 0)).GetContentResult();

            model.Locations = res.ToList();

            //Add theme images from results
            foreach (var d in model.Locations)
            {
                carousel.Items.Add(new CarouselItemBlock
                {
                    Image = d.Image,
                    Heading = d.Name,
                });
            }

            model.Carousel = carousel;

            return View(model);
        }
    }
}