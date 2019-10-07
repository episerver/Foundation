using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Pages;
using Foundation.Cms.Personalization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class ContentRecommendationsBlockController : BlockController<ContentRecommendationsBlock>
    {
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly ICmsTrackingService _recommendationService;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly IUrlResolver _urlResolver;

        public ContentRecommendationsBlockController(
            IPageRouteHelper pageRouteHelper,
            ICmsTrackingService recommendationService,
            IUrlResolver urlResolver,
            IContentTypeRepository contentTypeRepository)
        {
            _pageRouteHelper = pageRouteHelper;
            _recommendationService = recommendationService;
            _urlResolver = urlResolver;
            _contentTypeRepository = contentTypeRepository;
        }

        protected PageData CurrentPage => _pageRouteHelper.Page;

        public override ActionResult Index(ContentRecommendationsBlock currentBlock)
        {
            var request = new ContentRecommendationViewModel
            {
                ContentId = CurrentPage.ContentGuid.ToString(),
                SiteId = SiteDefinition.Current.Id.ToString(),
                LanguageId = CurrentPage.Language.Name,
                NumberOfRecommendations = currentBlock.NumberOfRecommendations
            };

            var model = new ContentRecommendationsBlockViewModel { ContentRecommendationItems = new List<ContentRecommendationItem>() };
            foreach (var item in Task.Run(async () => await _recommendationService.GetRecommendationContent(HttpContext, request)).Result)
            {
                var sitePageData = item.Content as FoundationPageData;
                if (sitePageData == null)
                {
                    continue;
                }
                model.ContentRecommendationItems.Add(new ContentRecommendationItem
                {
                    Content = sitePageData,
                    ContentUrl = _urlResolver.GetUrl(item.Content.ContentLink),
                    ContentType = _contentTypeRepository.Load(item.Content.ContentTypeID).Name
                }
                );
            }

            return PartialView("~/Features/Blocks/Views/ContentRecommendationsBlock.cshtml", model);
        }
    }
}
