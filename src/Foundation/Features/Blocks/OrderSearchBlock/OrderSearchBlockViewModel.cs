using Foundation.Commerce.Customer;
using Foundation.Features.Blocks.OrderSearchBlock;
using Foundation.Features.Shared;
using Mediachase.Commerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout.ViewModels
{
    public class OrderSearchBlockViewModel : BlockViewModel<OrderSearchBlock>
    {
        public List<OrderViewModel> Orders { get; set; }
        public FoundationContact CurrentCustomer { get; set; }
        public string OrderDetailUrl { get; set; }
        public OrderFilter Filter { get; set; }

        public OrderSearchBlockViewModel(OrderSearchBlock orderSearchBlock) : base(orderSearchBlock)
        {
            Orders = new List<OrderViewModel>();
            Filter = new OrderFilter();
        }
    }

    public class OrderFilter
    {
        public int CurrentBlockId { get; set; }
        public string Keyword { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int OrderStatusId { get; set; }
        public string PaymentMethodId { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }

        public List<KeyValuePair<string, string>> PaymentMethods { get; set; }
        public List<KeyValuePair<string, int>> OrderStatuses { get; set; }

        // OrderHistoryPage

        public string PurchaseOrderNumber { get; set; }
        public string OrderGroupId { get; set; }
        public string AddressId { get; set; }
        public List<KeyValuePair<string, string>> Addresses { get; set; }

        public OrderFilter()
        {
            PaymentMethods = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("All", "") };
            OrderStatuses = new List<KeyValuePair<string, int>>() { new KeyValuePair<string, int>("All", 0) };
            Addresses = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("All", "") };
        }

        public static void LoadDefault(OrderFilter filter, PaymentMethodViewModelFactory paymentMethodViewModelFactory)
        {
            filter.PaymentMethods.AddRange(paymentMethodViewModelFactory.GetPaymentMethodViewModels()
                .Select(x => new KeyValuePair<string, string>(x.SystemKeyword, x.PaymentMethodId.ToString())));

            filter.OrderStatuses.AddRange(OrderStatus.RegisteredStatuses.Select(x => new KeyValuePair<string, int>(x.Name, x.Id)));
        }
    }
}
