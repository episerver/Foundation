using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework.BestBets;
using EPiServer.Framework.Localization;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Find.Cms.ViewModels;
using Foundation.Find.Commerce;
using Foundation.Find.Commerce.ViewModels;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Foundation.Demo.ViewModels
{
    public class DemoSearchViewModelFactory : CommerceSearchViewModelFactory
    {
        private readonly ICommerceSearchService _searchService;
        private readonly LocalizationService _localizationService;
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly UrlResolver _urlResolver;
        private readonly HttpContextBase _httpContextBase;

        public DemoSearchViewModelFactory(LocalizationService localizationService, ICommerceSearchService searchService,
            IContentLoader contentLoader,
            ReferenceConverter referenceConverter,
            UrlResolver urlResolver,
            HttpContextBase httpContextBase) : base(localizationService, searchService, contentLoader, referenceConverter, urlResolver, httpContextBase)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _urlResolver = urlResolver;
            _httpContextBase = httpContextBase;
            _localizationService = localizationService;
        }

        public override TModel Create<TModel, TContent>(TContent currentContent, IArgs args)
        {
            var commerceArgs = args as CommerceArgs;
            if (args == null)
            {
                return null;
            }

            var baseModel = new TModel();
            var model = baseModel as DemoSearchViewModel<TContent>;
            if (model == null)
            {
                return null;
            }

            model.CurrentContent = currentContent;
            if (!commerceArgs.FilterOption.Q.IsNullOrEmpty() && (commerceArgs.FilterOption.Q.StartsWith("*") || commerceArgs.FilterOption.Q.StartsWith("?")))
            {
                model.CurrentContent = currentContent;
                model.FilterOption = commerceArgs.FilterOption;
                model.HasError = true;
                model.ErrorMessage = _localizationService.GetString("/Search/BadFirstCharacter");
                model.CategoriesFilter = new CategoriesFilterViewModel();
                return baseModel;
            }

            var commerceFilterOption = commerceArgs.FilterOption;
            var results = _searchService.Search(currentContent, commerceFilterOption, commerceArgs.SelectedFacets, commerceArgs.CatalogId);

            commerceFilterOption.TotalCount = results.TotalCount;
            commerceFilterOption.FacetGroups = results.FacetGroups.ToList();

            commerceFilterOption.Sorting = _searchService.GetSortOrder().Select(x => new SelectListItem
            {
                Text = _localizationService.GetString("/Category/Sort/" + x.Name),
                Value = x.Name.ToString(),
                Selected = string.Equals(x.Name.ToString(), commerceFilterOption.Sort)
            });

            model.CurrentContent = currentContent;
            model.ProductViewModels = results?.ProductViewModels ?? new List<ProductTileViewModel>();
            model.FilterOption = commerceFilterOption;
            model.CategoriesFilter = GetCategoriesFilter(currentContent, commerceFilterOption.Q);
            model.DidYouMeans = results.DidYouMeans;
            model.Query = commerceFilterOption.Q;
            model.IsMobile = _httpContextBase.GetOverriddenBrowser().IsMobileDevice;

            return baseModel;
        }

        private CategoriesFilterViewModel GetCategoriesFilter(IContent currentContent, string query)
        {
            var bestBets = new BestBetRepository().List().Where(i => i.PhraseCriterion.Phrase.CompareTo(query) == 0);
            var ownStyleBestBets = bestBets.Where(i => i.BestBetSelector is CommerceBestBetSelector && i.HasOwnStyle);
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
            foreach (var nodeContent in _contentLoader.GetChildren<NodeContent>(catalog.ContentLink))
            {
                var nodeFilter = new CategoryFilter
                {
                    DisplayName = nodeContent.DisplayName,
                    Url = _urlResolver.GetUrl(nodeContent.ContentLink),
                    IsActive = currentContent != null && currentContent.ContentLink == nodeContent.ContentLink,
                    IsBestBet = ownStyleBestBets.Any(x => ((CommerceBestBetSelector)x.BestBetSelector).ContentLink.ID == nodeContent.ContentLink.ID)
                };
                viewModel.Categories.Add(nodeFilter);

                var nodeChildren = _contentLoader.GetChildren<NodeContent>(nodeContent.ContentLink);
                foreach (var nodeChild in nodeChildren)
                {
                    var nodeChildFilter = new CategoryFilter
                    {
                        DisplayName = nodeChild.DisplayName,
                        Url = _urlResolver.GetUrl(nodeChild.ContentLink),
                        IsActive = currentContent != null && currentContent.ContentLink == nodeChild.ContentLink,
                        IsBestBet = ownStyleBestBets.Any(x => ((CommerceBestBetSelector)x.BestBetSelector).ContentLink.ID == nodeChild.ContentLink.ID)
                    };

                    nodeFilter.Children.Add(nodeChildFilter);
                    if (nodeChildFilter.IsActive)
                    {
                        nodeFilter.IsActive = true;
                    }

                    var nodeChildrenOfNodeChild = _contentLoader.GetChildren<NodeContent>(nodeChild.ContentLink);
                    foreach (var nodeChildOfChild in nodeChildrenOfNodeChild)
                    {
                        var nodeChildOfChildFilter = new CategoryFilter
                        {
                            DisplayName = nodeChildOfChild.DisplayName,
                            Url = _urlResolver.GetUrl(nodeChildOfChild.ContentLink),
                            IsActive = currentContent != null && currentContent.ContentLink == nodeChildOfChild.ContentLink,
                            IsBestBet = ownStyleBestBets.Any(x => ((CommerceBestBetSelector)x.BestBetSelector).ContentLink.ID == nodeChildOfChild.ContentLink.ID)
                        };

                        nodeChildFilter.Children.Add(nodeChildOfChildFilter);
                        if (nodeChildOfChildFilter.IsActive)
                        {
                            nodeFilter.IsActive = nodeChildFilter.IsActive = true;
                        }
                    }
                }

            }
            return viewModel;
        }
    }
}
