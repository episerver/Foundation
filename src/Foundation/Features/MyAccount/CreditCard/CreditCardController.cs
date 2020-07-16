using EPiServer;
using EPiServer.Web.Mvc;
using Foundation.Commerce;
using Foundation.Commerce.Customer.Services;
using Foundation.Features.MyOrganization.Organization;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.CreditCard
{
    /// <summary>
    /// Manage credit cards of user and organization
    /// </summary>
    [Authorize]
    public class CreditCardController : PageController<CreditCardPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICreditCardService _creditCardService;
        private readonly IOrganizationService _organizationService;
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Construct credit card controller
        /// </summary>
        /// <param name="contentLoader">Service to load content</param>
        /// <param name="creditCardService">Service to manipulate credit card data</param>
        /// <param name="organizationService">Service to manipulate organization data</param>
        /// <param name="customerService">Service to manipute </param>
        public CreditCardController(
            IContentLoader contentLoader,
            ICreditCardService creditCardService,
            IOrganizationService organizationService,
            ICustomerService customerService)
        {
            _contentLoader = contentLoader;
            _creditCardService = creditCardService;
            _organizationService = organizationService;
            _customerService = customerService;
        }

        /// <summary>
        /// List all credit card of current user
        /// </summary>
        /// <param name="currentPage">Current credit card page</param>
        /// <returns></returns>
        public ActionResult Index(CreditCardPage currentPage) => currentPage.B2B ? B2B(currentPage) : List(currentPage);

        /// <summary>
        /// List all credit card of current user, with b2b navigation on view
        /// </summary>
        /// <param name="currentPage">Current credit card page</param>
        /// <returns></returns>
        [NavigationAuthorize("Admin")]
        public ActionResult B2B(CreditCardPage currentPage) => List(currentPage);

        /// <summary>
        /// List all credit card of current user
        /// </summary>
        /// <param name="currentPage">Current credit card page</param>
        /// <returns></returns>
        private ActionResult List(CreditCardPage currentPage)
        {
            var model = new CreditCardCollectionViewModel(currentPage)
            {
                CurrentContent = currentPage,
                CreditCards = new List<CreditCardModel>(),
                CurrentContact = _customerService.GetCurrentContact(),
                IsB2B = currentPage.B2B
            };
            model.CreditCards = _creditCardService.List(currentPage.B2B, false);
            return View("Index", model);
        }

        /// <summary>
        /// Remove credit card by id
        /// </summary>
        /// <param name="creditCardId">Credit card id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(string creditCardId)
        {
            _creditCardService.Delete(creditCardId);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Add/Edit Credit card of current customer or current organization
        /// </summary>
        /// <param name="currentPage">Current credit card page</param>
        /// <param name="creditCardId">Credit card id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditForm(CreditCardPage currentPage, string creditCardId) => currentPage.B2B ? CreditCardEditViewB2B(currentPage, creditCardId) : CreditCardEditView(currentPage, creditCardId);

        /// <summary>
        /// Add/Edit Credit card of current customer or current organization
        /// </summary>
        /// <param name="currentPage">Current credit card page</param>
        /// <param name="creditCardId">Credit card id</param>
        /// <returns></returns>
        [NavigationAuthorize("Admin")]
        private ActionResult CreditCardEditViewB2B(CreditCardPage currentPage, string creditCardId) => CreditCardEditView(currentPage, creditCardId);

        /// <summary>
        /// Add/Edit Credit card of current customer or current organization
        /// </summary>
        /// <param name="currentPage">Current credit card page</param>
        /// <param name="creditCardId">Credit card id</param>
        /// <returns></returns>
        private ActionResult CreditCardEditView(CreditCardPage currentPage, string creditCardId)
        {
            var viewModel = new CreditCardViewModel(currentPage)
            {
                CreditCard = new CreditCardModel
                {
                    CreditCardId = creditCardId
                },
                CurrentContent = currentPage,
                IsB2B = currentPage.B2B
            };

            if (currentPage.B2B)
            {
                viewModel.Organizations = viewModel.GetAllOrganizationAndSub(_organizationService.GetCurrentFoundationOrganization());
            }

            if (_creditCardService.IsValid(viewModel.CreditCard.CreditCardId, out var errorMessage))
            {
                _creditCardService.LoadCreditCard(viewModel.CreditCard);
            }
            else
            {
                viewModel.ErrorMessage = errorMessage;
            }
            ViewData["IsReadOnly"] = false;

            return View("EditForm", viewModel);
        }

        /// <summary>
        /// Save credit card
        /// </summary>
        /// <param name="viewModel">data model of credit card</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CreditCardViewModel viewModel)
        {
            _creditCardService.Save(viewModel.CreditCard);
            return RedirectToAction("Index");
        }
    }
}