using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Statistics;
using EPiServer.Framework.Localization;
using Foundation.Cms.Extensions;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Foundation.Find.Cms.ViewModels
{
    public class CmsSearchViewModelFactory : ISearchViewModelFactory
    {
        private readonly ICmsSearchService _searchService;
        private readonly LocalizationService _localizationService;
        private readonly HttpContextBase _httpContextBase;
        private readonly IClient _findClient;

        public CmsSearchViewModelFactory(LocalizationService localizationService,
            ICmsSearchService searchService,
            HttpContextBase httpContextBase,
            IClient findClient)
        {
            _searchService = searchService;
            _httpContextBase = httpContextBase;
            _localizationService = localizationService;
            _findClient = findClient;
        }

        public TModel Create<TModel, TContent>(TContent currentContent, IArgs args)
            where TContent : IContent
            where TModel : CmsSearchViewModel<TContent>, new()
        {
            var model = new TModel();
            model.CurrentContent = currentContent;

            var cmsArgs = args as CmsArgs;
            if (args == null)
            {
                return null;
            }

            if (cmsArgs.FilterOption.Q != null && (cmsArgs.FilterOption.Q.StartsWith("*") || cmsArgs.FilterOption.Q.StartsWith("?")))
            {
                model.CurrentContent = currentContent;
                model.FilterOption = cmsArgs.FilterOption;
                model.HasError = true;
                model.ErrorMessage = _localizationService.GetString("/Search/BadFirstCharacter");
                return model;
            }

            model.ContentSearchResult = _searchService.SearchContent(cmsArgs.FilterOption);
            model.CurrentContent = currentContent;
            model.FilterOption = cmsArgs.FilterOption;
            model.Query = cmsArgs.FilterOption.Q;
            model.IsMobile = _httpContextBase.GetOverriddenBrowser().IsMobileDevice;

            if (!model.ContentSearchResult.Hits.Any() && model.FilterOption.Q.IsNullOrEmpty())
            {
                model.DidYouMeans = _findClient.Statistics().GetDidYouMean(model.FilterOption.Q);
            }

            return model;
        }
    }
}
