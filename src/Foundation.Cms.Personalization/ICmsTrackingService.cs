﻿using EPiServer.Core;
using EPiServer.Personalization.CMS.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Cms.Personalization
{
    public interface ICmsTrackingService
    {
        Task HeroBlockClicked(HttpContextBase context, string blockId, string blockName, string pageName);
        Task VideoBlockViewed(HttpContextBase context, string blockId, string blockName, string pageName);
        Task SearchedKeyword(HttpContextBase httpContextBase, string keyword);
        Task BlockViewed(BlockData block, IContent page, HttpContextBase httpContext);
        Task ImageViewed(ImageData image, IContent page, HttpContextBase httpContext);
        Task<IEnumerable<RecommendationResult>> GetRecommendationContent(HttpContextBase httpContext, ContentRecommendationViewModel recommendationRequest);
    }
}