// EPiServer.Find removed: IClient (Find) dependency removed. GetCategoriesFilter uses ContentLoader instead.
// Phase 4 will restore full implementation using Optimizely Graph.
using Foundation.Features.CatalogContent;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;

namespace Foundation.Features.Search
{
    public interface ISearchViewModelFactory
    {
        SearchViewModel<TContent> Create<TContent>(TContent currentContent, string selectedFacets,
            int catlogId, FilterOptionViewModel filterOption)
            where TContent : IContent;
    }

    public class SearchViewModelFactory : ISearchViewModelFactory
    {
        private readonly ISearchService _searchService;
        private readonly LocalizationService _localizationService;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly UrlResolver _urlResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SearchViewModelFactory(LocalizationService localizationService, ISearchService searchService,
            IContentLoader contentLoader,
            ReferenceConverter referenceConverter,
            UrlResolver urlResolver,
            IHttpContextAccessor httpContextAccessor)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _urlResolver = urlResolver;
            _httpContextAccessor = httpContextAccessor;
            _localizationService = localizationService;
        }

        public virtual SearchViewModel<TContent> Create<TContent>(TContent currentContent,
            string selectedFacets,
            int catalogId,
            FilterOptionViewModel filterOption)
            where TContent : IContent
        {
            var model = new SearchViewModel<TContent>(currentContent);

            if (!filterOption.Q.IsNullOrEmpty() && (filterOption.Q.StartsWith("*") || filterOption.Q.StartsWith("?")))
            {
                model.CurrentContent = currentContent;
                model.FilterOption = filterOption;
                model.HasError = true;
                model.ErrorMessage = _localizationService.GetString("/Search/BadFirstCharacter");
                model.CategoriesFilter = new CategoriesFilterViewModel();
                return model;
            }

            var results = _searchService.Search(currentContent, filterOption, selectedFacets, catalogId);

            filterOption.TotalCount = results.TotalCount;
            filterOption.FacetGroups = results.FacetGroups.ToList();

            filterOption.Sorting = _searchService.GetSortOrder().Select(x => new SelectListItem
            {
                Text = _localizationService.GetString("/Category/Sort/" + x.Name),
                Value = x.Name.ToString(),
                Selected = string.Equals(x.Name.ToString(), filterOption.Sort)
            });

            model.CurrentContent = currentContent;
            model.ProductViewModels = results?.ProductViewModels ?? new List<ProductTileViewModel>();
            model.FilterOption = filterOption;
            model.CategoriesFilter = GetCategoriesFilter(currentContent, filterOption.Q);
            model.DidYouMeans = results.DidYouMeans;
            model.Query = filterOption.Q;
            var detection = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IDetectionService>();
            model.IsMobile = detection.Device.Type == Device.Mobile;

            return model;
        }

        private CategoriesFilterViewModel GetCategoriesFilter(IContent currentContent, string query)
        {
            // EPiServer.Find removed: category filter now uses ContentLoader instead of Find index.
            // Phase 4 will restore full Find/Graph implementation.
            var catalogId = 0;
            var node = currentContent as NodeContent;
            if (node != null)
            {
                catalogId = node.CatalogId;
            }
            var catalog = _contentLoader.GetChildren<CatalogContentBase>(_referenceConverter.GetRootLink())
                .FirstOrDefault(x => catalogId == 0 || x.CatalogId == catalogId);

            if (catalog == null)
            {
                return new CategoriesFilterViewModel();
            }

            var viewModel = new CategoriesFilterViewModel();
            var nodes = _contentLoader.GetChildren<NodeContent>(catalog.ContentLink);

            foreach (var nodeContent in nodes)
            {
                var nodeFilter = new CategoryFilter
                {
                    DisplayName = nodeContent.DisplayName,
                    Url = _urlResolver.GetUrl(nodeContent.ContentLink),
                    IsActive = currentContent != null && currentContent.ContentLink == nodeContent.ContentLink,
                    IsBestBet = false
                };

                // Load children (e.g. Mens Shoes, Mens Jackets)
                foreach (var childNode in _contentLoader.GetChildren<NodeContent>(nodeContent.ContentLink))
                {
                    var childFilter = new CategoryFilter
                    {
                        DisplayName = childNode.DisplayName,
                        Url = _urlResolver.GetUrl(childNode.ContentLink),
                        IsActive = currentContent != null && currentContent.ContentLink == childNode.ContentLink,
                        IsBestBet = false
                    };

                    // Load grandchildren (third level)
                    foreach (var grandchildNode in _contentLoader.GetChildren<NodeContent>(childNode.ContentLink))
                    {
                        childFilter.Children.Add(new CategoryFilter
                        {
                            DisplayName = grandchildNode.DisplayName,
                            Url = _urlResolver.GetUrl(grandchildNode.ContentLink),
                            IsActive = currentContent != null && currentContent.ContentLink == grandchildNode.ContentLink,
                            IsBestBet = false
                        });
                    }

                    nodeFilter.Children.Add(childFilter);
                }

                viewModel.Categories.Add(nodeFilter);
            }
            return viewModel;
        }
    }
}
