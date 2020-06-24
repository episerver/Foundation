using Foundation.Commerce;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Extensions;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Mediachase.Commerce.Orders.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Features.Checkout.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly ICustomerService _customerService;
        private readonly ICartService _cartService;
        private readonly HttpContextBase _httpContext;

        public PaymentService(ICustomerService customerService,
            ICartService cartService,
            HttpContextBase httpContext)
        {
            _customerService = customerService;
            _cartService = cartService;
            _httpContext = httpContext;
        }

        public IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode)
        {
            var methods = PaymentManager.GetPaymentMethodsByMarket(marketId)
                .PaymentMethod
                .Where(x => x.IsActive && languageCode.Equals(x.LanguageId, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Ordering)
                .Select(x => new PaymentMethodViewModel
                {
                    PaymentMethodId = x.PaymentMethodId,
                    SystemKeyword = x.SystemKeyword,
                    FriendlyName = x.Name,
                    Description = x.Description,
                    IsDefault = x.IsDefault
                });

            if (_httpContext == null || !EPiServer.Security.PrincipalInfo.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return methods.Where(payment => !payment.SystemKeyword.Equals(Constant.Order.BudgetPayment));
            }

            var currentContact = _customerService.GetCurrentContact();
            if (string.IsNullOrEmpty(currentContact.UserRole))
            {
                return methods.Where(payment => !payment.SystemKeyword.Equals(Constant.Order.BudgetPayment));
            }

            var cart = _cartService.LoadCart(_cartService.DefaultCartName, true)?.Cart;
            if (cart != null && cart.IsQuoteCart() && currentContact.B2BUserRole == B2BUserRoles.Approver)
            {
                return methods.Where(payment => payment.SystemKeyword.Equals(Constant.Order.BudgetPayment));
            }

            return currentContact.B2BUserRole == B2BUserRoles.Purchaser ? methods : methods.Where(payment => !payment.SystemKeyword.Equals(Constant.Order.BudgetPayment));
        }
    }
}