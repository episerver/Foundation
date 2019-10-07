using EPiServer;
using EPiServer.Core;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.Order.ViewModels;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.SubscriptionHistory
{
    public class SubscriptionHistoryController : PageController<SubscriptionHistoryPage>
    {
        private readonly IContentLoader _contentLoader;

        public SubscriptionHistoryController(IContentLoader contentLoader) => _contentLoader = contentLoader;

        public ActionResult Index(SubscriptionHistoryPage currentPage)
        {
            var paymentPlans = OrderContext.Current.LoadByCustomerId<PaymentPlan>(PrincipalInfo.CurrentPrincipal.GetContactId())
                .OrderBy(x => x.Created)
                .ToList();

            var viewModel = new SubscriptionHistoryViewModel(currentPage)
            {
                CurrentContent = currentPage,
                PaymentPlans = paymentPlans
            };

            viewModel.PaymentPlanDetailsPageUrl = UrlResolver.Current.GetUrl(_contentLoader.Get<CommerceHomePage>(ContentReference.StartPage).PaymentPlanDetailsPage);

            return View(viewModel);
        }
    }
}