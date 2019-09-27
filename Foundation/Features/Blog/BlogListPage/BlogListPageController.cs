using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Blog.BlogListPage
{
    public class BlogListPageController : PageController<Cms.Pages.BlogListPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly ICmsTrackingService _trackingService;

        public BlogListPageController(IContentLoader contentLoader,
            UrlResolver urlResolver,
            ICmsTrackingService trackingService)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Cms.Pages.BlogListPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = new BlogListPageViewModel(currentPage);
            model.SubNavigation = GetSubNavigation(currentPage);
            return View(model);
        }

        private List<KeyValuePair<string, string>> GetSubNavigation(Cms.Pages.BlogListPage currentPage)
        {
            var subNavigation = new List<KeyValuePair<string, string>>();
            var childrenPages = _contentLoader.GetChildren<PageData>(currentPage.ContentLink).Select(x => x as Cms.Pages.BlogListPage).Where(x => x != null);
            var siblingPages = _contentLoader.GetChildren<PageData>(currentPage.ParentLink).Select(x => x as Cms.Pages.BlogListPage).Where(x => x != null);

            if (siblingPages != null && siblingPages.Count() > 0)
            {
                subNavigation.AddRange(siblingPages.Select(x => new KeyValuePair<string, string>(x.MetaTitle, x.PublicUrl(_urlResolver))));
            }

            // when current page is blog start page
            if (childrenPages != null && childrenPages.Count() > 0)
            {
                subNavigation.AddRange(childrenPages.Select(x => new KeyValuePair<string, string>(x.MetaTitle, x.PublicUrl(_urlResolver))));
            }

            return subNavigation;
        }
    }
}
