using Mediachase.Commerce.Customers;
using Newtonsoft.Json;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Commerce.GiftCard
{
    public class GiftCardManagerController : Controller
    {
        private readonly IGiftCardService _giftCardService;

        public GiftCardManagerController(IGiftCardService giftCardService)
        {
            _giftCardService = giftCardService;
        }

        [HttpGet]
        //[MenuItem("/global/extensions/giftcards", TextResourceKey = "/Shared/GiftCards", SortIndex = 300)]
        public ActionResult Index() => View();

        [HttpGet]
        public ContentResult GetAllGiftCards()
        {
            var data = _giftCardService.GetAllGiftCards();
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json"
            };
        }

        [HttpPost]
        public string AddGiftCard(GiftCard giftCard) => _giftCardService.CreateGiftCard(giftCard);

        [HttpPost]
        public string UpdateGiftCard(GiftCard giftCard) => _giftCardService.UpdateGiftCard(giftCard);

        [HttpPost]
        public string DeleteGiftCard(string giftCardId) => _giftCardService.DeleteGiftCard(giftCardId);

        [HttpGet]
        public ContentResult GetAllContacts()
        {
            var data = CustomerContext.Current.GetContacts(0, 1000).Select(c => new
            {
                ContactId = c.PrimaryKeyId.ToString(),
                ContactName = c.FullName
            });

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json"
            };
        }
    }
}