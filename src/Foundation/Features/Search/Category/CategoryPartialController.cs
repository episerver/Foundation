using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Find.Facets;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Search.Category
{
    [TemplateDescriptor(Inherited = true)]
    public class CategoryPartialController : PartialContentController<GenericNode>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;

        public CategoryPartialController(ISearchViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public override ActionResult Index(GenericNode currentContent)
        {
            var viewmodel = GetSearchModel(currentContent);
            return PartialView("_Category", viewmodel);
        }

        protected virtual SearchViewModel<GenericNode> GetSearchModel(GenericNode currentContent)
        {
            return _viewModelFactory.Create(currentContent, HttpContext.Request.QueryString["facets"], 0, new FilterOptionViewModel
            {
                FacetGroups = new List<FacetGroupOption>(),
                Page = 1,
                PageSize = currentContent.PartialPageSize
            });
        }
    }
}