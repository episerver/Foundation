using EPiServer.Core;
using EPiServer.Security;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Settings;
using Foundation.Features.Settings;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.SubscriptionHistory
{
    public class SubscriptionHistoryController : PageController<SubscriptionHistoryPage>
    {
        private readonly ISettingsService _settingsService;

        public SubscriptionHistoryController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

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

            viewModel.PaymentPlanDetailsPageUrl = UrlResolver.Current.GetUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.PaymentPlanDetailsPage ?? ContentReference.StartPage);

            return View(viewModel);
        }
    }
}