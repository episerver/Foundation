using EPiServer.Commerce.Marketing;
using Foundation.Cms;
using System.Collections.Generic;

namespace Foundation.Commerce.Marketing
{
    public class PromotionsViewModel
    {
        public PromotionsViewModel()
        {
            Promotions = new List<PromotionData>();
            PagingInfo = new PagingInfo();
        }

        public PagingInfo PagingInfo { get; set; }
        public List<PromotionData> Promotions { get; set; }
    }
}