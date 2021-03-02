using EPiServer.Commerce.Marketing;
using EPiServer.Security;
using Mediachase.Commerce.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Marketing
{
    public class FoundationCouponFilter : ICouponFilter
    {
        private readonly IUniqueCouponService _couponService;

        public FoundationCouponFilter(IUniqueCouponService couponService) => _couponService = couponService;

        public PromotionFilterContext Filter(PromotionFilterContext filterContext, IEnumerable<string> couponCodes)
        {
            var codes = couponCodes.ToList();
            _ = PrincipalInfo.CurrentPrincipal?.GetCustomerContact()?.Email;

            foreach (var includedPromotion in filterContext.IncludedPromotions)
            {
                var couponCode = includedPromotion.Coupon.Code;
                var uniqueCodes = _couponService.GetByPromotionId(includedPromotion.ContentLink.ID);
                if (string.IsNullOrEmpty(couponCode) && !(uniqueCodes?.Any() ?? false))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(couponCode))
                {
                    CheckSingleCoupon(filterContext, codes, couponCode, includedPromotion);
                }
                else
                {
                    CheckMultipleCoupons(filterContext, codes, includedPromotion, uniqueCodes);
                }
            }

            return filterContext;
        }

        protected virtual IEqualityComparer<string> GetCodeEqualityComparer() => StringComparer.OrdinalIgnoreCase;

        private void CheckSingleCoupon(PromotionFilterContext filterContext, IEnumerable<string> couponCodes, string couponCode, PromotionData includedPromotion)
        {
            if (couponCodes.Contains(couponCode, GetCodeEqualityComparer()))
            {
                filterContext.AddCouponCode(includedPromotion.ContentGuid, couponCode);
            }
            else
            {
                filterContext.ExcludePromotion(
                    includedPromotion,
                    FulfillmentStatus.CouponCodeRequired,
                    filterContext.RequestedStatuses.HasFlag(RequestFulfillmentStatus.NotFulfilled));
            }
        }

        private void CheckMultipleCoupons(PromotionFilterContext filterContext, IList<string> couponCodes, PromotionData includedPromotion, List<UniqueCoupon> uniqueCoupons)
        {
            foreach (var couponCode in uniqueCoupons)
            {
                // Check if the code its assigned to the user and that has not been used
                if (couponCodes.Contains(couponCode.Code, GetCodeEqualityComparer()) && couponCode.UsedRedemptions < couponCode.MaxRedemptions)
                {
                    filterContext.AddCouponCode(includedPromotion.ContentGuid, couponCode.Code);
                    return;
                }
            }

            filterContext.ExcludePromotion(includedPromotion, FulfillmentStatus.CouponCodeRequired,
                filterContext.RequestedStatuses.HasFlag(RequestFulfillmentStatus.NotFulfilled));
        }
    }
}
