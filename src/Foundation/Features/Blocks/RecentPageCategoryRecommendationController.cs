using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Globalization;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Blocks;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels.Blocks;
using Foundation.Find.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class RecentPageCategoryRecommendationController : BlockController<RecentPageCategoryRecommendationBlock>
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IClient _findClient;
        private readonly Random _random = new Random();
        private readonly IContentLoader _contentLoader;
        private readonly ICmsSearchService _cmsSearchService;
        private readonly IContentTypeRepository<PageType> _contentTypeRepository;
        private readonly IPageRouteHelper _pageRouteHelper;

        public RecentPageCategoryRecommendationController(CategoryRepository categoryRepository,
            IClient findClient,
            IContentLoader contentLoader,
            ICmsSearchService cmsSearchService,
            IContentTypeRepository<PageType> contentTypeRepository,
            IPageRouteHelper pageRouteHelper)
        {
            _categoryRepository = categoryRepository;
            _findClient = findClient;
            _contentLoader = contentLoader;
            _cmsSearchService = cmsSearchService;
            _contentTypeRepository = contentTypeRepository;
            _pageRouteHelper = pageRouteHelper;
        }

        public override ActionResult Index(RecentPageCategoryRecommendationBlock currentBlock)
        {
            var categories = Cms.Extensions.ContentExtensions.GetPageBrowseHistory()
                .Reverse()
                .Where(x => x is FoundationPageData)
                .Select(x => x as FoundationPageData)
                .SelectMany(x => x.Categories)
                .ToList();

            var pages = new List<FoundationPageData>();
            var rootFilterId = currentBlock.FilterRoot != null ? currentBlock.FilterRoot.ID : ContentReference.RootPage.ID;
            var pageTypesFilterId = currentBlock.FilterTypes?.Split(',').ToList().Select(x => int.Parse(x));
            var query = _findClient.Search<FoundationPageData>()
                    .Filter(x => x.Language.Name.Match(ContentLanguage.PreferredCulture.Name))
                    .Filter(x => x.Ancestors().Match(rootFilterId.ToString()))
                    .Filter(x => !x.ContentLink.ID.Match(_pageRouteHelper.Page.ContentLink.ID));

            if (categories.Any())
            {
                query = _cmsSearchService.FilterByCategories(query, categories);
            }

            if (pageTypesFilterId != null && pageTypesFilterId.Count() > 0)
            {
                query = query.Filter(x => x.ContentTypeID.In(pageTypesFilterId));
            }

            pages = query.GetContentResult().Items.ToList();

            var model = new RecentPageCategoryRecommendationViewModel(currentBlock)
            {
                CategoryPages = new List<CategoryViewModel>()
            };

            if (!pages.Any())
            {
                if (ContentReference.IsNullOrEmpty(currentBlock.FilterRoot))
                {
                    return PartialView("~/Features/Blocks/Views/RecentPageCategoryRecommendation.cshtml", model);
                }

                pages = _contentLoader.GetChildren<FoundationPageData>(currentBlock.FilterRoot).Where(x => x.ContentLink.ID != _pageRouteHelper.Page.ContentLink.ID).ToList();
            }

            var pageCount = pages.Count;
            var count = Math.Max(currentBlock.NumberOfRecommendations, pageCount);
            if (count > currentBlock.NumberOfRecommendations)
            {
                count = currentBlock.NumberOfRecommendations;
            }
            foreach (var page in PickSomeInRandomOrder(pages, count))
            {
                var categoryCount = page.Categories?.Count;
                var categoryInt = 0;

                if (categories.Any())
                {
                    categoryInt = categories[_random.Next(0, categories.Count - 1)].ID;
                }

                if (categoryInt == 0 && categoryCount > 0)
                {
                    categoryInt = page.Categories[_random.Next(0, categoryCount.Value - 1)].ID;
                }

                var category = _categoryRepository.Get(categoryInt);
                if (category != null)
                {
                    model.CategoryPages.Add(new CategoryViewModel
                    {
                        Id = category.ID,
                        Name = category.Name,
                        Page = page
                    });
                }
                else
                {
                    model.CategoryPages.Add(new CategoryViewModel
                    {
                        Page = page
                    });
                }

            }
            return PartialView("~/Features/Blocks/Views/RecentPageCategoryRecommendation.cshtml", model);
        }

        private IEnumerable<T> PickSomeInRandomOrder<T>(IEnumerable<T> someTypes,
            int maxCount)
        {
            var randomSortTable = new Dictionary<double, T>();

            foreach (var someType in someTypes)
            {
                randomSortTable[_random.NextDouble()] = someType;
            }

            return randomSortTable.OrderBy(kvp => kvp.Key).Take(maxCount).Select(kvp => kvp.Value);
        }
    }
}
