using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find.Commerce;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Mediachase.Commerce.Catalog;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.CategoryBlock
{
    [TemplateDescriptor(Default = true)]
    public class CategoryBlockController : BlockController<CategoryBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly UrlResolver _urlResolver;

        public CategoryBlockController(IContentLoader contentLoader, UrlResolver urlResolver, ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _urlResolver = urlResolver;
        }

        public override ActionResult Index(CategoryBlock currentBlock)
        {
            var categories = !ContentReference.IsNullOrEmpty(currentBlock.Catalog)
                ? _contentLoader.GetChildren<NodeContent>(currentBlock.Catalog)
                : _contentLoader.GetChildren<NodeContent>(_referenceConverter.GetRootLink());

            var model = new CategoryBlockViewModel(currentBlock)
            {
                Heading = currentBlock.Heading,
                CategoryItems = categories.Select(ToViewModel).ToList()
            };

            return PartialView("~/Features/Blocks/CategoryBlock/CategoryBlock.cshtml", model);
        }

        private CategoryItemViewModel ToViewModel(NodeContent model)
        {
            var children = _contentLoader.GetChildren<NodeContent>(model.ContentLink);

            return new CategoryItemViewModel
            {
                Name = model.DisplayName,
                ImageUrl = model.DefaultImageUrl(),
                Uri = _urlResolver.GetUrl(model.ContentLink),
                ChildLinks = children.Select(
                    x => new CategoryChildLinkViewModel
                    {
                        Text = x.DisplayName,
                        Uri = _urlResolver.GetUrl(x.ContentLink)
                    })
                    .ToList()
            };
        }
    }
}