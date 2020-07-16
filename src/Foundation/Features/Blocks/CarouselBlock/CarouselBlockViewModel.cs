using EPiServer.Core;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.CarouselBlock
{
    public class CarouselBlockViewModel : BlockViewModel<CarouselBlock>
    {
        public CarouselBlockViewModel(CarouselBlock currentBlock) : base(currentBlock)
        {
            Items = new List<CarouselItem>();
        }

        public List<CarouselItem> Items { get; set; }
    }

    public class CarouselItem
    {
        public CarouselImage CarouselImage { get; set; }
        public IBlockViewModel<HeroBlock.HeroBlock> HeroBlock { get; set; }
    }

    public class CarouselImage
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public ContentReference Image { get; set; }
        public ContentReference ItemURL { get; set; }
    }
}
