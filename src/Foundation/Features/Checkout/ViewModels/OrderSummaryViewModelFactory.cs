using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using Foundation.Commerce.Markets;
using Mediachase.Commerce;
using System.Linq;

namespace Foundation.Features.Checkout.ViewModels
{
    public class OrderSummaryViewModelFactory
    {
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly ICurrencyService _currencyService;

        public OrderSummaryViewModelFactory(
            IOrderGroupCalculator orderGroupCalculator,
            ICurrencyService currencyService)
        {
            _orderGroupCalculator = orderGroupCalculator;
            _currencyService = currencyService;
        }

        public virtual OrderSummaryViewModel CreateOrderSummaryViewModel(ICart cart)
        {
            if (cart == null)
            {
                return CreateEmptyOrderSummaryViewModel();
            }

            var totals = _orderGroupCalculator.GetOrderGroupTotals(cart);

            return new OrderSummaryViewModel
            {
                SubTotal = totals.SubTotal,
                CartTotal = totals.Total,
                ShippingTotal = totals.ShippingTotal,
                ShippingSubtotal = _orderGroupCalculator.GetShippingSubTotal(cart),
                OrderDiscountTotal = _orderGroupCalculator.GetOrderDiscountTotal(cart),
                ShippingDiscountTotal = cart.GetShippingDiscountTotal(),
                ShippingTaxTotal = totals.ShippingTotal + totals.TaxTotal,
                TaxTotal = totals.TaxTotal,
                PaymentTotal = cart.Currency.Round(totals.Total.Amount - (cart.GetFirstForm().Payments?.Sum(x => x.Amount) ?? 0)),
                OrderDiscounts = cart.GetFirstForm().Promotions.Where(x => x.DiscountType == DiscountType.Order).Select(x => new OrderDiscountViewModel
                {
                    Discount = new Money(x.SavedAmount, new Currency(cart.Currency)),
                    DisplayName = x.Description
                })
            };
        }

        private OrderSummaryViewModel CreateEmptyOrderSummaryViewModel()
        {
            var zeroAmount = new Money(0, _currencyService.GetCurrentCurrency());
            return new OrderSummaryViewModel
            {
                CartTotal = zeroAmount,
                OrderDiscountTotal = zeroAmount,
                ShippingDiscountTotal = zeroAmount,
                ShippingSubtotal = zeroAmount,
                ShippingTaxTotal = zeroAmount,
                ShippingTotal = zeroAmount,
                SubTotal = zeroAmount,
                TaxTotal = zeroAmount,
                OrderDiscounts = Enumerable.Empty<OrderDiscountViewModel>(),
            };
        }
    }
}