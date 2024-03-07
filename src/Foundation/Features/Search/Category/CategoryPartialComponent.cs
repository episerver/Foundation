using Foundation.Infrastructure.Find.Facets;

namespace Foundation.Features.Search.Category
{
    public class CategoryPartialComponent : AsyncPartialContentComponent<GenericNode>
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
        protected override async Task<IViewComponentResult> InvokeComponentAsync(GenericNode currentContent)
        {
            var viewmodel = GetSearchModel(currentContent);
            return await Task.FromResult(View("_Category", viewmodel));
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