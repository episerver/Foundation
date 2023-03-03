using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Foundation.Features.Media;

namespace Foundation.Features.Blocks.CarouselBlock
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class CarouselBlockContentTypeCheckInitialization : IInitializableModule
    {
        private IContentEvents _contentEvents = null;
        private IContentLoader _contentLoader = null;

        public void Initialize(InitializationEngine initializationEngine)
        {
            ServiceProviderHelper serviceLocationHelper = initializationEngine.Locate;
            _contentEvents = initializationEngine.Locate.Advanced.GetInstance<IContentEvents>();
            _contentLoader = serviceLocationHelper.ContentLoader();
            _contentEvents.PublishingContent += Events_PublishingContent;
        }

        public void Uninitialize(InitializationEngine initializationEngine)
        {
            _contentEvents.PublishingContent -= Events_PublishingContent;
        }

        private void Events_PublishingContent(object sender, ContentEventArgs e)
        {
            if (e?.Content is CarouselBlock carouselBlock)
            {
                bool cancelPublish = false;
                string cancelReason = "";

                var model = new CarouselBlockViewModel(carouselBlock);
                if (carouselBlock.CarouselItems != null)
                {
                    foreach (var contentAreaItem in carouselBlock.CarouselItems.FilteredItems)
                    {
                        var carouselItem = _contentLoader.Get<IContentData>(contentAreaItem.ContentLink);
                        if (carouselItem is not ImageMediaData && carouselItem is not ImageData && carouselItem is not HeroBlock.HeroBlock && carouselItem is not ContainerBlock.ContainerBlock)
                        {
                            cancelPublish = true;
                            cancelReason = " Carousel Block only allows images, Hero blocks and Containers. Please remove other asset types.";
                        }
                    }
                }
                if (cancelPublish)
                {
                    e.CancelAction = true;
                    e.CancelReason = cancelReason;
                }
            }
        }
    }
}
