using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.DataAbstraction;
using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;

namespace Foundation.Features.Pages.ExtendedStandardPage
{
    public class ExtendedStandardPageController : PageController<Cms.Pages.ExtendedStandardPage>
    {
        private readonly CategoryRepository _categoryRepository;

        public ExtendedStandardPageController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [PageViewTracking]
        public ActionResult Index(Cms.Pages.ExtendedStandardPage currentPage)
        {
            var model = ExtendedStandardPageViewModel.Create(currentPage, _categoryRepository);
            return View(model);
        }
    }
}