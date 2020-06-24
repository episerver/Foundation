using EPiServer.Commerce.Marketing;
using Foundation.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Marketing
{
    public class PromotionCouponsViewModel
    {
        public PromotionCouponsViewModel()
        {
            Coupons = new List<UniqueCoupon>();
            PagingInfo = new PagingInfo();
        }

        public PromotionData Promotion { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public List<UniqueCoupon> Coupons { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Valid From")]
        public DateTime ValidFrom { get; set; }

        public DateTime? Expiration { get; set; }

        [Required]
        [Display(Name = "Max Redemptions")]
        public int MaxRedemptions { get; set; }
        public int PromotionId { get; set; }
    }
}