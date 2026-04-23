namespace Foundation.Features.MyOrganization.Orders
{
    public class OrderOrganizationViewModel
    {
        public string OrderNumber { get; set; }
        public string SubOrganization { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public string Ammount { get; set; }
        public string PlacedOrderDate { get; set; }
        public string Currency { get; set; }
        public int OrderGroupId { get; set; }
        public bool IsOrganizationOrder { get; set; }
        public bool IsPaymentApproved { get; set; }
        public bool IsQuoteOrder { get; set; }
    }
}