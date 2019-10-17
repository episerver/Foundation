using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Markets
{
    public class MarketContentLoader
    {
        private readonly CampaignInfoExtractor _campaignInfoExtractor;
        private readonly IContentLoader _contentLoader;
        private readonly PromotionProcessorResolver _promotionProcessorResolver;

        public MarketContentLoader(
            IContentLoader contentLoader,
            CampaignInfoExtractor campaignInfoExtractor,
            PromotionProcessorResolver promotionProcessorResolver)
        {
            _contentLoader = contentLoader;

            _campaignInfoExtractor = campaignInfoExtractor;
            _promotionProcessorResolver = promotionProcessorResolver;
        }

        public virtual IEnumerable<PromotionItems> GetPromotionItemsForMarket(IMarket market)
        {
            return GetEvaluablePromotionsInPriorityOrderForMarket(market)
                .Select(promotion =>
                    _promotionProcessorResolver.ResolveForPromotion(promotion).GetPromotionItems(promotion));
        }

        public virtual IList<PromotionData> GetEvaluablePromotionsInPriorityOrderForMarket(IMarket market) => GetPromotions().Where(x => IsValid(x, market)).OrderBy(x => x.Priority).ToList();

        public virtual IEnumerable<PromotionData> GetPromotions()
        {
            var campaigns = _contentLoader.GetChildren<SalesCampaign>(GetCampaignFolderRoot());
            var promotions = new List<PromotionData>();

            foreach (var campaign in campaigns)
            {
                promotions.AddRange(_contentLoader.GetChildren<PromotionData>(campaign.ContentLink));
            }

            return promotions;
        }

        protected virtual ContentReference GetCampaignFolderRoot() => SalesCampaignFolder.CampaignRoot;

        private bool IsValid(PromotionData promotion, IMarket market)
        {
            var campaign = _contentLoader.Get<SalesCampaign>(promotion.ParentLink);

            return IsActive(promotion, campaign) && IsValidMarket(campaign, market);
        }

        private bool IsActive(PromotionData promotion, SalesCampaign campaign)
        {
            var status = _campaignInfoExtractor.GetEffectiveStatus(promotion, campaign);

            return status == CampaignItemStatus.Active;
        }

        private static bool IsValidMarket(SalesCampaign campaign, IMarket market)
        {
            if (market == null)
            {
                return true;
            }

            if (!market.IsEnabled)
            {
                return false;
            }

            return campaign.TargetMarkets?.Contains(market.MarketId.Value, StringComparer.OrdinalIgnoreCase) ?? false;
        }
    }
}