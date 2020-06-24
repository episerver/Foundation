using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using System;
using System.Linq;
using System.Web.Mvc;


namespace Foundation.Features.Blocks.ProductHeroBlock
{
    [TemplateDescriptor(Default = true)]
    public class ProductHeroBlockController : BlockController<ProductHeroBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public ProductHeroBlockController(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        public override ActionResult Index(ProductHeroBlock currentBlock)
        {
            var imageUrl = string.Empty;
            var imagePosition = string.Empty;


            if (currentBlock.Image.Product != null)
            {
                var entryContentBase = _contentLoader.Get<EntryContentBase>(currentBlock.Image.Product.Items[0].ContentLink);
                imageUrl = entryContentBase.GetAssets<IContentImage>(_contentLoader, _urlResolver).FirstOrDefault() ?? string.Empty;
            }

            if (currentBlock.Image.ImagePosition.Equals("ImageLeft", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "justify-content: flex-start;";
            }
            else if (currentBlock.Image.ImagePosition.Equals("ImageCenter", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "justify-content: center;";
            }
            else if (currentBlock.Image.ImagePosition.Equals("ImageRight", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "justify-content: flex-end;";
            }
            else if (currentBlock.Image.ImagePosition.Equals("ImagePaddings", StringComparison.OrdinalIgnoreCase))
            {
                imagePosition = "padding: "
                    + currentBlock.Image.PaddingTop + "px "
                    + currentBlock.Image.PaddingRight + "px "
                    + currentBlock.Image.PaddingBottom + "px "
                    + currentBlock.Image.PaddingLeft + "px;";
            }

            var model = new ProductHeroBlockViewModel(currentBlock)
            {
                ImageUrl = imageUrl,
                ImagePosition = imagePosition
            };

            return PartialView("~/Features/Blocks/Views/ProductHeroBlock.cshtml", model);
        }
    }
}
