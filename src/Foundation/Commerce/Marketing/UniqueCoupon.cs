using System;

namespace Foundation.Commerce.Marketing
{
    public class UniqueCoupon
    {
        public long Id { get; set; }
        public int PromotionId { get; set; }
        public string Code { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? Expiration { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime Created { get; set; }
        public int MaxRedemptions { get; set; }
        public int UsedRedemptions { get; set; }
    }
}