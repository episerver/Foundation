using EPiServer.Security;
using Foundation.Infrastructure.Cms.Settings;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Security;

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