using EPiServer.Globalization;
using Foundation.Infrastructure.Cms;
using System.Text;

namespace Foundation.Infrastructure.Commerce.Marketing
{
    public class SingleUseCouponController : Controller
    {
        private readonly IContentLoader _contentLoader;
        private readonly IUniqueCouponService _couponService;

        public SingleUseCouponController(IContentLoader contentLoader,
            IUniqueCouponService couponService)
        {
            _contentLoader = contentLoader;
            _couponService = couponService;
        }

        
        [HttpGet]
        [Route("episerver/foundation/promotions", Name = "promotions")]
        public ActionResult Index()
        {
            var promotions = GetPromotions(_contentLoader.GetDescendents(GetCampaignRoot()))
                .ToList();

            return View("/Infrastructure/Commerce/Views/SingleUseCoupon/Index.cshtml", new PromotionsViewModel
            {
                Promotions = promotions
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("episerver/foundation/promotions", Name = "promotionsPost")]
        public ActionResult Index(PagingInfo pagingInfo)
        {
            var promotions = GetPromotions(_contentLoader.GetDescendents(GetCampaignRoot()))
                .Skip(pagingInfo.PageNumber * pagingInfo.PageSize)
                .Take(pagingInfo.PageSize)
                .ToList();

            return View(new PromotionsViewModel
            {
                Promotions = promotions
            });
        }

        [HttpGet]
        [Route("episerver/foundation/editPromotionCoupons", Name = "editPromotionCoupons")]
        public ActionResult EditPromotionCoupons(int id)
        {
            var promotion = _contentLoader.Get<PromotionData>(new ContentReference(id));
            var coupons = _couponService.GetByPromotionId(id);

            return View("/Infrastructure/Commerce/Views/SingleUseCoupon/EditPromotionCoupons.cshtml", new PromotionCouponsViewModel
            {
                Coupons = coupons ?? new List<UniqueCoupon>(),
                Promotion = promotion,
                PromotionId = promotion.ContentLink.ID,
                MaxRedemptions = 1
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("episerver/foundation/updateOrDeleteCoupon", Name = "updateOrDeleteCoupon")]
        public string UpdateOrDeleteCoupon([FromForm] UniqueCoupon model)
        {
            if (model.ActionType.Equals("update", StringComparison.Ordinal))
            {
                var updated = false;
                var coupon = _couponService.GetById(model.Id);

                if (coupon != null)
                {
                    coupon.Code = model.Code;
                    coupon.Expiration = model.Expiration;
                    coupon.MaxRedemptions = model.MaxRedemptions;
                    coupon.UsedRedemptions = model.UsedRedemptions;
                    coupon.ValidFrom = model.ValidFrom;
                    updated = _couponService.SaveCoupons(new List<UniqueCoupon> { coupon });
                }

                return updated ? "update_ok" : "update_nok";
            }
            else
            {
                var deleted = _couponService.DeleteById(model.Id);
                return deleted ? "delete_ok" : "delete_nok";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("episerver/foundation/generateCoupon", Name = "generateCoupon")]
        public ActionResult Generate([FromForm] PromotionCouponsViewModel model)
        {
            var couponRecords = new List<UniqueCoupon>();
            for (var i = 0; i < model.Quantity; i++)
            {
                couponRecords.Add(new UniqueCoupon
                {
                    Code = _couponService.GenerateCoupon(),
                    Created = DateTime.UtcNow,
                    Expiration = model.Expiration,
                    MaxRedemptions = model.MaxRedemptions,
                    PromotionId = model.PromotionId,
                    UsedRedemptions = 0,
                    ValidFrom = model.ValidFrom
                });
            }

            _couponService.SaveCoupons(couponRecords);
            return RedirectToRoute("editPromotionCoupons", new { id = model.PromotionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("episerver/foundation/deleteAllCoupons", Name = "deleteAllCoupons")]
        public ActionResult DeleteAll([FromForm] PromotionCouponsViewModel model)
        {
            var deleted = _couponService.DeleteByPromotionId(model.PromotionId);
            return RedirectToRoute("editPromotionCoupons", new { id = model.PromotionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("episerver/foundation/downloadCoupons", Name = "downloadCoupons")]
        public FileResult Download([FromForm] PromotionCouponsViewModel model)
        {
            var coupons = _couponService.GetByPromotionId(model.PromotionId);

            var sb = new StringBuilder();

            //Headers

            sb.Append($"PromotionId,Code,ValidFrom,Expiration,CustomerId,MaxRedemptions,UsedRedemptions");
            sb.Append("\r\n");
            for (int i = 0; i < coupons.Count; i++)
            {
                sb.Append($"{coupons[i].PromotionId}," +
                    $"{coupons[i].Code}," +
                    $"{coupons[i].ValidFrom}," +
                    $"{coupons[i].Expiration}," +
                    $"{coupons[i].CustomerId}," +
                    $"{coupons[i].MaxRedemptions}," +
                    $"{coupons[i].UsedRedemptions}");
                sb.Append("\r\n");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"{model.PromotionId}.csv");
        }

        private ContentReference GetCampaignRoot()
        {
            return _contentLoader.GetChildren<SalesCampaignFolder>(ContentReference.RootPage)
                .FirstOrDefault()?.ContentLink ?? ContentReference.EmptyReference;
        }

        private List<PromotionData> GetPromotions(IEnumerable<ContentReference> references)
        {
            return _contentLoader.GetItems(references, ContentLanguage.PreferredCulture)
                .OfType<PromotionData>()
                .ToList();
        }
    }
}