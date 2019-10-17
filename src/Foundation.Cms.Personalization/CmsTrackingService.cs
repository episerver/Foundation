using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Logging;
using EPiServer.Personalization;
using EPiServer.Personalization.CMS.Model;
using EPiServer.Personalization.CMS.Recommendation;
using EPiServer.Security;
using EPiServer.Tracking.Core;
using EPiServer.Tracking.PageView;
using EPiServer.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Cms.Personalization
{
    public class CmsTrackingService : ICmsTrackingService
    {
        private readonly IContentLoader _contentLoader;
        private readonly ITrackingService _trackingService;
        private readonly IContextModeResolver _contextModeResolver;
        private readonly ISiteDefinitionResolver _siteDefinitionResolver;
        private readonly IRecommendationService _personalizationRecommendationService;
        private readonly ILogger _logger = LogManager.GetLogger(typeof(CmsTrackingService));

        public CmsTrackingService(IContentLoader contentLoader,
            ITrackingService trackingService,
            IContextModeResolver contextModeResolver,
            ISiteDefinitionResolver siteDefinitionResolver,
            IRecommendationService personalizationRecommendationService)
        {
            _contentLoader = contentLoader;
            _trackingService = trackingService;
            _contextModeResolver = contextModeResolver;
            _siteDefinitionResolver = siteDefinitionResolver;
            _personalizationRecommendationService = personalizationRecommendationService;
        }

        public virtual async System.Threading.Tasks.Task PageViewed(HttpContextBase context, PageData pageData)
        {
            if (pageData == null || !PageIsInViewMode())
            {
                await System.Threading.Tasks.Task.CompletedTask;
            }

            var site = _siteDefinitionResolver.GetByContent(pageData.ContentLink, true);
            var ancesctors = _contentLoader.GetAncestors(pageData.ContentLink).Select(c => c.ContentGuid).ToArray();

            try
            {
                var user = new UserData
                {
                    Name = PrincipalInfo.CurrentPrincipal?.Identity?.Name,
                    Email = EPiServerProfile.Current?.Email,
                };

                var pageDataTrackingModel = new EpiPageViewWrapper
                {
                    Epi = new EpiPageView
                    {
                        ContentGuid = pageData.ContentGuid,
                        Language = pageData.Language?.Name,
                        SiteId = site.Id,
                        Ancestors = ancesctors,
                    }
                };

                var trackingData = new TrackingData<EpiPageViewWrapper>
                {
                    EventType = "epiPageView",
                    PageTitle = pageData.Name,
                    User = user,
                    Value = $"Viewed {pageData.Name}",
                    Payload = pageDataTrackingModel
                };

                await _trackingService.Track(trackingData, context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        public virtual async System.Threading.Tasks.Task VideoBlockViewed(HttpContextBase context, string blockId, string blockName, string pageName)
        {
            try
            {
                var trackingData = new TrackingData<dynamic>
                {
                    EventType = "epiVideoBlockView",
                    Value = "Video Block viewed: '" + blockName + "' on page - '" + pageName + "'",
                    PageUri = context.Request.UrlReferrer.AbsoluteUri,
                    PageTitle = pageName,
                    Payload = new
                    {
                        BlockId = blockId,
                        BlockName = blockName
                    }
                };

                await _trackingService.Track(trackingData, context);
            }
            catch
            {
            }
        }

        public virtual async System.Threading.Tasks.Task HeroBlockClicked(HttpContextBase context, string blockId, string blockName, string pageName)
        {
            try
            {
                var trackingData = new TrackingData<dynamic>
                {
                    EventType = "epiHeroBlockClick",
                    Value = "Hero Block clicked: '" + blockName + "' on page - '" + pageName + "'",
                    PageUri = context.Request.UrlReferrer.AbsoluteUri,
                    PageTitle = pageName,
                    Payload = new
                    {
                        BlockId = blockId,
                        BlockName = blockName
                    }
                };

                await _trackingService.Track(trackingData, context);
            }
            catch
            {
            }
        }

        public virtual async System.Threading.Tasks.Task BlockViewed(BlockData block, IContent page, HttpContextBase httpContext)
        {
            try
            {
                var trackingData = new TrackingData<BlockView>
                {
                    EventType = typeof(BlockView).Name,
                    Value = "Block viewed: '" + (block as IContent).Name + "' on page - '" + page.Name + "'",
                    PageUri = PageEditing.GetEditUrl(page.ContentLink),
                    Payload = new BlockView
                    {
                        PageName = page.Name,
                        PageId = page.ContentLink.ID,
                        BlockId = (block as IContent).ContentLink.ID,
                        BlockName = (block as IContent).Name
                    }
                };

                await _trackingService.Track(trackingData, httpContext);
            }
            catch
            {
            }

        }

        public virtual async System.Threading.Tasks.Task ImageViewed(ImageData image, IContent page, HttpContextBase httpContext)
        {
            try
            {
                var trackingData = new TrackingData<ImageView>
                {
                    EventType = "Imagery",
                    Value = "Image viewed: '" + (image as IContent).Name + "' on page - '" + page.Name + "'",
                    PageUri = PageEditing.GetEditUrl(page.ContentLink),
                    Payload = new ImageView
                    {
                        PageName = page.Name,
                        PageId = page.ContentLink.ID,
                        ImageId = (image as IContent).ContentLink.ID,
                        ImageName = (image as IContent).Name
                    }
                };

                await _trackingService.Track(trackingData, httpContext);
            }
            catch
            {
            }
        }

        public virtual async System.Threading.Tasks.Task SearchedKeyword(HttpContextBase httpContextBase, string keyword)
        {
            await _trackingService.Track(new TrackingData<dynamic>
            {
                EventType = "epiSearch",
                Value = $"Searched {keyword}",
            }, httpContextBase);
        }

        public virtual async System.Threading.Tasks.Task<IEnumerable<RecommendationResult>> GetRecommendationContent(HttpContextBase httpContext,
           ContentRecommendationViewModel requestModel)
        {
            var recommendationRequest = new RecommendationRequest
            {
                siteId = requestModel.SiteId,
                context = new Context { contentId = requestModel.ContentId, languageId = requestModel.LanguageId },
                numberOfRecommendations = requestModel.NumberOfRecommendations == 0 ? 3 : requestModel.NumberOfRecommendations
            };

            return await _personalizationRecommendationService.Get(httpContext, recommendationRequest);
        }

        private bool PageIsInViewMode()
        {
            return _contextModeResolver.CurrentMode == ContextMode.Default;
        }
    }
}