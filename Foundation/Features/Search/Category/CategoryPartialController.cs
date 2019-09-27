using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Models.Catalog;
using Foundation.Demo.ViewModels;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.ViewModels;
using Foundation.Find.Commerce.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Search.Category
{
    [TemplateDescriptor(Inherited = true)]
    public class CategoryPartialController : PartialContentController<GenericNode>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;

        public CategoryPartialController(ISearchViewModelFactory viewModelFactory) => _viewModelFactory = viewModelFactory;

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public override ActionResult Index(GenericNode currentContent)
        {
            var viewmodel = GetSearchModel(currentContent);
            return PartialView("_Category", viewmodel);
        }

        protected virtual DemoSearchViewModel<GenericNode> GetSearchModel(GenericNode currentContent)
        {
            return _viewModelFactory.Create<DemoSearchViewModel<GenericNode>, GenericNode>(currentContent, new CommerceArgs
            {
                FilterOption = new CommerceFilterOptionViewModel
                {
                    FacetGroups = new List<FacetGroupOption>(),
                    Page = 1,
                    PageSize = currentContent.PartialPageSize
                },
                SelectedFacets = HttpContext.Request.QueryString["facets"],
                CatalogId = 0
            });
        }
    }
}