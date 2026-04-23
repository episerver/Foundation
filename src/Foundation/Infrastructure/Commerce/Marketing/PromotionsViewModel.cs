using Foundation.Infrastructure.Cms;

namespace Foundation.Infrastructure.Commerce.Marketing
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