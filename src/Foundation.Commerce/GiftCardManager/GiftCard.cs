namespace Foundation.Commerce.GiftCard
{
    public class GiftCard
    {
        public string GiftCardId { get; set; }
        public string ContactId { get; set; }
        public string GiftCardName { get; set; }
        public string ContactName { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal RemainBalance { get; set; }
        public bool IsActive { get; set; }
        public string RedemptionCode { get; set; }
    }
}