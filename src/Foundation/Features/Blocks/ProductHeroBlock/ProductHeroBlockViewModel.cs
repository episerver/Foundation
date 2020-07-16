using Foundation.Features.Shared;

namespace Foundation.Features.Blocks.ProductHeroBlock
{
    public class ProductHeroBlockViewModel : BlockViewModel<ProductHeroBlock>
    {
        public ProductHeroBlockViewModel(ProductHeroBlock currentBlock) : base(currentBlock)
        {
        }

        public string ImageUrl { get; set; }
        public string ImagePosition { get; set; }
    }
}
