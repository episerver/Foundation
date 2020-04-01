using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Tracking.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Personalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class TrackingBlockController : BlockController<TrackingBlock>
    {
        private readonly ITrackingService _trackingService;
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly HttpContextBase _httpContextBase;
        private readonly ICmsTrackingService _cmsTrackingService;

        public TrackingBlockController(
            ITrackingService trackingService,
            IPageRouteHelper pageRouteHelper,
            HttpContextBase httpContextBase,
            ICmsTrackingService cmsTrackingService)
        {
            _trackingService = trackingService;
            _pageRouteHelper = pageRouteHelper;
            _httpContextBase = httpContextBase;
            _cmsTrackingService = cmsTrackingService;
        }

        public override ActionResult Index(TrackingBlock currentBlock)
        {
            _cmsTrackingService.CustomViewed(_pageRouteHelper.Page, currentBlock, _httpContextBase);

            return PartialView("~/Features/Blocks/Views/TrackingBlock.cshtml", currentBlock);
        }



    }
}
