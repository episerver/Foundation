using Foundation.Features.Checkout.Services;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Commerce.Customer.Services;

namespace Foundation.Features.MyOrganization.Orders
{
    [Authorize]
    public class OrdersController : PageController<OrdersPage>
    {
        private readonly ICustomerService _customerService;
        private readonly IOrdersService _ordersService;
        private readonly ISettingsService _settingsService;

        public OrdersController(ICustomerService customerService,
            IOrdersService ordersService,
            ISettingsService settingsService)
        {
            _customerService = customerService;
            _ordersService = ordersService;
            _settingsService = settingsService;
        }

        public ActionResult Index(OrdersPage currentPage)
        {
            var organizationUsersList = _customerService.GetContactsForOrganization();
            var viewModel = new OrdersPageViewModel
            {
                CurrentContent = currentPage
            };

            var ordersOrganization = new List<OrderOrganizationViewModel>();
            foreach (var user in organizationUsersList)
            {
                ordersOrganization.AddRange(_ordersService.GetUserOrders(user.ContactId));
            }
            viewModel.OrdersOrganization = ordersOrganization;

            viewModel.OrderDetailsPageUrl =
                UrlResolver.Current.GetUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.OrderDetailsPage ?? ContentReference.StartPage);
            return View(viewModel);
        }

        public ActionResult QuickOrder(OrdersPage currentPage)
        {
            var viewModel = new OrdersPageViewModel { CurrentContent = currentPage };
            return View(viewModel);
        }
    }
}