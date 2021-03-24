using System.Collections.Generic;

namespace Foundation.Commerce.Marketing
{
    public interface IUniqueCouponService
    {
        bool SaveCoupons(List<UniqueCoupon> coupons);

        bool DeleteById(long id);

        bool DeleteByPromotionId(int id);

        List<UniqueCoupon> GetByPromotionId(int id);

        UniqueCoupon GetById(long id);

        string GenerateCoupon();

        bool DeleteExpiredCoupons();
    }
}
