using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Home;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization.Orders
{
    [Authorize]
    public class OrdersController : PageController<OrdersPage>
    {
        private readonly ICustomerService _customerService;
        private readonly IOrdersService _ordersService;
        private readonly IContentLoader _contentLoader;

        public OrdersController(ICustomerService customerService, IOrdersService ordersService, IContentLoader contentLoader)
        {
            _customerService = customerService;
            _ordersService = ordersService;
            _contentLoader = contentLoader;
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
                UrlResolver.Current.GetUrl(_contentLoader.Get<HomePage>(ContentReference.StartPage).OrderDetailsPage);
            return View(viewModel);
        }

        public ActionResult QuickOrder(OrdersPage currentPage)
        {
            var viewModel = new OrdersPageViewModel { CurrentContent = currentPage };
            return View(viewModel);
        }
    }
}