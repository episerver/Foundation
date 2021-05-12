using EPiServer.Web.Mvc;
using Foundation.Infrastructure.Find.Facets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Foundation.Features.Search.Category
{
    public class CategoryPartialComponent : PartialContentComponent<GenericNode>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryPartialComponent(ISearchViewModelFactory viewModelFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _viewModelFactory = viewModelFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [AcceptVerbs(new string[] { "GET", "POST" })]
        public override IViewComponentResult Invoke(GenericNode currentContent)
        {
            var viewmodel = GetSearchModel(currentContent);
            return View("_Category", viewmodel);
        }

        protected virtual SearchViewModel<GenericNode> GetSearchModel(GenericNode currentContent)
        {
            return _viewModelFactory.Create(currentContent, _httpContextAccessor.HttpContext.Request.Query["facets"].ToString(), 0, new FilterOptionViewModel
            {
                FacetGroups = new List<FacetGroupOption>(),
                Page = 1,
                PageSize = currentContent.PartialPageSize
            });
        }
    }
}