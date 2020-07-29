using EPiServer.Commerce.Order;
using Foundation.Features.Checkout.Payments;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization;
using Foundation.Features.Shared;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Checkout.ViewModels
{
    [Bind(Exclude = "Payment")]
    public class CheckoutViewModel : ContentViewModel<CheckoutPage>
    {
        public const string MultiShipmentCheckoutViewName = "MultiShipmentCheckout";

        public const string SingleShipmentCheckoutViewName = "SingleShipmentCheckout";

        public CheckoutViewModel()
        {
            Payments = new List<PaymentOptionBase>();
        }

        public CheckoutViewModel(CheckoutPage checkoutPage) : base(checkoutPage)
        {
            Payments = new List<PaymentOptionBase>();
        }

        /// <summary>
        /// Gets or sets a collection of all coupon codes that have been applied.
        /// </summary>
        public IEnumerable<string> AppliedCouponCodes { get; set; }

        /// <summary>
        /// Gets or sets all available payment methods that the customer can choose from.
        /// </summary>
        public IEnumerable<PaymentMethodViewModel> PaymentMethodViewModels { get; set; }

        public string ReferrerUrl { get; set; }

        /// <summary>
        /// Gets or sets all existing shipments related to the current order.
        /// </summary>
        public IList<ShipmentViewModel> Shipments { get; set; }

        /// <summary>
        /// Gets or sets a list of all existing addresses for the current customer and that can be used for billing and shipment.
        /// </summary>
        public IList<AddressModel> AvailableAddresses { get; set; }

        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        public AddressModel BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the payment method associated to the current purchase.
        /// </summary>
        public IList<PaymentOptionBase> Payments { get; set; }

        public IPaymentMethod Payment { get; set; }

        /// <summary>
        /// Gets or sets whether the billing address should be the same as the shipping address.
        /// </summary>
        public bool UseShippingingAddressForBilling { get; set; }

        /// <summary>
        /// Gets or sets the view message.
        /// </summary>
        public string Message { get; set; }

        public int BillingAddressType { get; set; }

        public Currency Currency { get; set; }

        public string SelectedPayment { get; set; }

        public OrderSummaryViewModel OrderSummary { get; set; }

        /// <summary>
        /// Gets the name of the checkout view required depending on the number of distinct shipping addresses.
        /// </summary>
        public string ViewName => Shipments.Count > 1 ? MultiShipmentCheckoutViewName : SingleShipmentCheckoutViewName;

        public ContactViewModel CurrentCustomer { get; set; }
        public string QuoteStatus { get; set; } = "";
        public bool IsOnHoldBudget { get; set; }

        public bool IsUsePaymentPlan { get; set; }

        public PaymentPlanSetting PaymentPlanSetting { get; set; }
    }

    public class PaymentPlanSetting
    {
        public PaymentPlanCycle CycleMode
        {
            get; set;
        }

        public int CycleLength { get; set; }
        public int MaxCyclesCount { get; set; }
        public int CompletedCyclesCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? LastTransactionDate { get; set; }
        public bool IsActive { get; set; }
    }
}