using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Features.Media;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foundation.Features.Blocks.CarouselBlock
{
    public class CarouselBlockComponent : AsyncBlockComponent<CarouselBlock>
    {
        private readonly IContentLoader _contentLoader;

        public CarouselBlockComponent(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(CarouselBlock currentBlock)
        {
            var model = new CarouselBlockViewModel(currentBlock);

            if (currentBlock.CarouselItems != null)
            {
                foreach (var contentAreaItem in currentBlock.CarouselItems.FilteredItems)
                {
                    var carouselItem = _contentLoader.Get<IContentData>(contentAreaItem.ContentLink);

                    if (carouselItem is ImageMediaData)
                    {
                        var carouselImage = new CarouselImage()
                        {
                            Heading = ((ImageMediaData)carouselItem).Title,
                            Description = ((ImageMediaData)carouselItem).Description,
                            Image = ((ImageMediaData)carouselItem).ContentLink
                        };

                        model.Items.Add(new CarouselItem() { CarouselImage = carouselImage });
                    }
                    else if (carouselItem is HeroBlock.HeroBlock)
                    {
                        model.Items.Add(new CarouselItem() { HeroBlock = new BlockViewModel<HeroBlock.HeroBlock>((HeroBlock.HeroBlock)carouselItem) });
                    }
                }
            }

            return await Task.FromResult(View("~/Features/Blocks/CarouselBlock/CarouselBlock.cshtml", model));
        }
    }
}
