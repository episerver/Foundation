using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Tracking.Core;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Personalization
{
    public interface ICmsTrackingService
    {
        Task HeroBlockClicked(HttpContextBase context, string blockId, string blockName, string pageName);
        Task VideoBlockViewed(HttpContextBase context, string blockId, string blockName, string pageName);
        Task SearchedKeyword(HttpContextBase httpContextBase, string keyword);
        Task BlockViewed(BlockData block, IContent page, HttpContextBase httpContext);
        Task ImageViewed(ImageData image, IContent page, HttpContextBase httpContext);
    }

    public class CmsTrackingService : ICmsTrackingService
    {
        private readonly ITrackingService _trackingService;

        public CmsTrackingService(ITrackingService trackingService) => _trackingService = trackingService;

        public virtual async Task VideoBlockViewed(HttpContextBase context, string blockId, string blockName, string pageName)
        {
            try
            {
                var trackingData = new TrackingData<dynamic>
                {
                    EventType = "epiVideoBlockView",
                    Value = "Video Block viewed: '" + blockName + "' on page - '" + pageName + "'",
                    PageUri = context.Request.UrlReferrer.AbsoluteUri,
                    PageTitle = pageName,
                    Payload = new
                    {
                        BlockId = blockId,
                        BlockName = blockName
                    }
                };

                await _trackingService.Track(trackingData, context);
            }
            catch
            {
            }
        }

        public virtual async Task HeroBlockClicked(HttpContextBase context, string blockId, string blockName, string pageName)
        {
            try
            {
                var trackingData = new TrackingData<dynamic>
                {
                    EventType = "epiHeroBlockClick",
                    Value = "Hero Block clicked: '" + blockName + "' on page - '" + pageName + "'",
                    PageUri = context.Request.UrlReferrer.AbsoluteUri,
                    PageTitle = pageName,
                    Payload = new
                    {
                        BlockId = blockId,
                        BlockName = blockName
                    }
                };

                await _trackingService.Track(trackingData, context);
            }
            catch
            {
            }
        }

        public virtual async Task BlockViewed(BlockData block, IContent page, HttpContextBase httpContext)
        {
            try
            {
                var trackingData = new TrackingData<BlockView>
                {
                    EventType = typeof(BlockView).Name,
                    Value = "Block viewed: '" + (block as IContent).Name + "' on page - '" + page.Name + "'",
                    PageUri = PageEditing.GetEditUrl(page.ContentLink),
                    Payload = new BlockView
                    {
                        PageName = page.Name,
                        PageId = page.ContentLink.ID,
                        BlockId = (block as IContent).ContentLink.ID,
                        BlockName = (block as IContent).Name
                    }
                };

                await _trackingService.Track(trackingData, httpContext);
            }
            catch
            {
            }
        }

        public virtual async Task ImageViewed(ImageData image, IContent page, HttpContextBase httpContext)
        {
            try
            {
                var trackingData = new TrackingData<ImageView>
                {
                    EventType = "Imagery",
                    Value = "Image viewed: '" + (image as IContent).Name + "' on page - '" + page.Name + "'",
                    PageUri = PageEditing.GetEditUrl(page.ContentLink),
                    Payload = new ImageView
                    {
                        PageName = page.Name,
                        PageId = page.ContentLink.ID,
                        ImageId = (image as IContent).ContentLink.ID,
                        ImageName = (image as IContent).Name
                    }
                };

                await _trackingService.Track(trackingData, httpContext);
            }
            catch
            {
            }
        }

        public virtual async Task SearchedKeyword(HttpContextBase httpContextBase, string keyword)
        {
            await _trackingService.Track(new TrackingData<dynamic>
            {
                EventType = "epiSearch",
                Value = $"Searched {keyword}",
            }, httpContextBase);
        }
    }
}