using EPiServer.Web.Mvc;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using Mediachase.Commerce.Customers;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.GiftCardPage
{
    /// <summary>
    /// A page to list all gift card belonging to a customer
    /// </summary>
    public class GiftCardPageController : PageController<Commerce.Models.Pages.GiftCardPage>
    {
        private readonly IGiftCardService _giftCardService;

        public GiftCardPageController(IGiftCardService giftCardService) => _giftCardService = giftCardService;

        public ActionResult Index(Commerce.Models.Pages.GiftCardPage currentPage)
        {
            var model = new GiftCardViewModel(currentPage)
            {
                CurrentContent = currentPage,
                GiftCardList = _giftCardService.GetCustomerGiftCards(CustomerContext.Current.CurrentContactId.ToString()).ToList()
            };

            return View(model);
        }
    }
}