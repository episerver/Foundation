using EPiServer;
using EPiServer.Commerce.Marketing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    public class FoundationCouponUsage : ICouponUsage
    {
        private readonly UniqueCouponService _uniqueCouponService;
        private readonly IContentRepository _contentRepository;

        public FoundationCouponUsage(IContentRepository contentRepository,
            UniqueCouponService uniqueCouponService)
        {
            _contentRepository = contentRepository;
            _uniqueCouponService = uniqueCouponService;
        }

        public void Report(IEnumerable<PromotionInformation> appliedPromotions)
        {
            foreach (var promotion in appliedPromotions)
            {
                var content = _contentRepository.Get<PromotionData>(promotion.PromotionGuid);
                CheckMultiple(content, promotion);
            }
        }

        private void CheckMultiple(PromotionData promotion, PromotionInformation promotionInformation)
        {
            var uniqueCodes = _uniqueCouponService.GetByPromotionId(promotion.ContentLink.ID);
            if (uniqueCodes == null || !uniqueCodes.Any())
            {
                return;
            }

            var uniqueCode = uniqueCodes.FirstOrDefault(x =>
                x.Code.Equals(promotionInformation.CouponCode, StringComparison.OrdinalIgnoreCase));
            if (uniqueCode == null)
            {
                return;
            }

            uniqueCode.UsedRedemptions++;
            _uniqueCouponService.SaveCoupons(new List<UniqueCoupon> { uniqueCode });
        }
    }
}
