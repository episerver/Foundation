using EPiServer.Commerce.Order;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.OrderHistory;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
{
    public class OrderViewModel
    {
        public IPurchaseOrder PurchaseOrder { get; set; }
        public IEnumerable<OrderHistoryItemViewModel> Items { get; set; }
        public AddressModel BillingAddress { get; set; }
        public IList<AddressModel> ShippingAddresses { get; set; }
        public string QuoteStatus { get; set; }
        public int OrderGroupId { get; set; }
        public Money OrderTotal { get; set; }
        public IList<IPayment> OrderPayments { get; set; }
    }
}