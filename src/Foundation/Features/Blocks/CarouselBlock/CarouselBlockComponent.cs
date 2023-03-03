using Foundation.Features.Media;

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
                    else if (carouselItem is ContainerBlock.ContainerBlock)
                    {
                        model.Items.Add(new CarouselItem() { ContainerBlock = new BlockViewModel<ContainerBlock.ContainerBlock>((ContainerBlock.ContainerBlock)carouselItem) });
                    }
                    else if (carouselItem is ImageData)
                    {
                        var carouselImage = new CarouselImage()
                        {
                            Heading = "",
                            Description = "",
                            Image = ((ImageData)carouselItem).ContentLink

                        };

                        model.Items.Add(new CarouselItem() { CarouselImage = carouselImage });
                    }
                    //else // for any none-image or hero block
                    //{
                    //    var carouselImage = new CarouselImage()
                    //    {
                    //        Heading = "Not supported",
                    //        Description = "Only support images or hero blocks",
                    //        Image = ((MediaData)carouselItem).ContentLink
                    //    };

                    //    model.Items.Add(new CarouselItem() { CarouselImage = carouselImage });
                    //}

                }
            }

            return await Task.FromResult(View("~/Features/Blocks/CarouselBlock/CarouselBlock.cshtml", model));
        }
    }
}
