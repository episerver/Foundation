using Foundation.Cms.ViewModels;
using Foundation.Commerce.Blocks;

namespace Foundation.Commerce.ViewModels
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
