using System.Text;

namespace Foundation.Features.Blocks.ProductHeroBlock
{
    public class ProductHeroBlockComponent : AsyncBlockComponent<ProductHeroBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public ProductHeroBlockComponent(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(ProductHeroBlock currentBlock)
        {
            var imageUrl = string.Empty;
            var imagePosition = new StringBuilder();

            if (currentBlock.Image.Product != null)
            {
                var entryContentBase = _contentLoader.Get<EntryContentBase>(currentBlock.Image.Product.Items[0].ContentLink);
                imageUrl = entryContentBase.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? string.Empty;
            }

            if (currentBlock.Image.ImagePosition.Equals("ImageRight", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition.Append("justify-content: flex-end;");
            }
            else if (currentBlock.Image.ImagePosition.Equals("ImageCenter", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition.Append("justify-content: center;");
            }
            else
            {
                imagePosition.Append("justify-content: flex-start;");
            }

            imagePosition.Append("padding: "
                + currentBlock.Image.PaddingTop + "px "
                + currentBlock.Image.PaddingRight + "px "
                + currentBlock.Image.PaddingBottom + "px "
                + currentBlock.Image.PaddingLeft + "px;");

            var model = new ProductHeroBlockViewModel(currentBlock)
            {
                ImageUrl = imageUrl,
                ImagePosition = imagePosition.ToString()
            };

            return await Task.FromResult(View("~/Features/Blocks/ProductHeroBlock/ProductHeroBlock.cshtml", model));
        }
    }
}