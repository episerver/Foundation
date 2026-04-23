using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Infrastructure.Commerce;
using Foundation.Infrastructure.Commerce.Customer;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Mediachase.Commerce.Orders.Managers;

namespace Foundation.Features.Checkout.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly ICustomerService _customerService;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(ICustomerService customerService,
            ICartService cartService,
            IHttpContextAccessor httpContextAccessor)
        {
            _customerService = customerService;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
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

            if (_httpContextAccessor.HttpContext == null || !EPiServer.Security.PrincipalInfo.CurrentPrincipal.Identity.IsAuthenticated)
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