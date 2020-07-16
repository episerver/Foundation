using EPiServer.Commerce.Order;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.Shared;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
{
    public class OrderConfirmationViewModel<T> : ContentViewModel<T> where T : FoundationPageData
    {
        public OrderConfirmationViewModel(T orderConfirmationPage) : base(orderConfirmationPage)
        {
        }

        public bool HasOrder { get; set; }
        public string OrderId { get; set; }
        public IEnumerable<ILineItem> Items { get; set; }
        public AddressModel BillingAddress { get; set; }
        public IList<AddressModel> ShippingAddresses { get; set; }
        public IEnumerable<IPayment> Payments { get; set; }
        public Guid ContactId { get; set; }
        public DateTime Created { get; set; }
        public int OrderGroupId { get; set; }
        public string NotificationMessage { get; set; }
        public Currency Currency { get; set; }
        public Money HandlingTotal { get; set; }
        public Money ShippingSubTotal { get; set; }
        public Money ShippingDiscountTotal { get; set; }
        public Money ShippingTotal { get; set; }
        public Money TaxTotal { get; set; }
        public Money CartTotal { get; set; }
        public Money OrderLevelDiscountTotal { get; set; }
        public Money SubTotal { get; set; }
        public List<Dictionary<string, string>> FileUrls { get; set; }
        public List<Dictionary<string, string>> Keys { get; set; }
        public string ElevatedRole { get; set; }
    }
}