using EPiServer.Commerce.Order;
using Foundation.Commerce;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization;
using Foundation.Features.Shared;
using Mediachase.Commerce.Orders;
using System.Collections.Generic;

namespace Foundation.Features.MyAccount.OrderDetails
{
    public class OrderDetailsViewModel : ContentViewModel<OrderDetailsPage>
    {
        public ContactViewModel CurrentCustomer { get; set; }
        public IPurchaseOrder PurchaseOrder { get; set; }
        public IEnumerable<OrderDetailsItemViewModel> Items { get; set; }
        public AddressModel BillingAddress { get; set; }
        public IList<AddressModel> ShippingAddresses { get; set; }
        public string QuoteStatus { get; set; }
        public int OrderGroupId { get; set; }
        public IPayment BudgetPayment { get; set; }
        public string ErrorMessage { get; set; }

        public string OrderStatus
            =>
                !IsPaymentApproved
                    ? Constant.Order.PendingApproval
                    : QuoteStatus ?? PurchaseOrder.OrderStatus.ToString();

        public bool IsPaymentApproved => BudgetPayment == null || BudgetPayment.TransactionType.Equals(TransactionType.Capture.ToString());
        public bool IsOrganizationOrder => BudgetPayment != null || !string.IsNullOrEmpty(QuoteStatus);
    }
}