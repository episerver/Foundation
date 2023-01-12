using EPiServer.Find.Commerce;

namespace Foundation.Features.Blocks.CategoryBlock
{
    public class CategoryBlockComponent : AsyncBlockComponent<CategoryBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly UrlResolver _urlResolver;

        public CategoryBlockComponent(IContentLoader contentLoader, UrlResolver urlResolver, ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
            _urlResolver = urlResolver;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(CategoryBlock currentBlock)
        {
            var categories = !ContentReference.IsNullOrEmpty(currentBlock.Catalog)
                ? _contentLoader.GetChildren<NodeContent>(currentBlock.Catalog)
                : _contentLoader.GetChildren<NodeContent>(_referenceConverter.GetRootLink());

            var model = new CategoryBlockViewModel(currentBlock)
            {
                Heading = currentBlock.Heading,
                CategoryItems = categories.Select(ToViewModel).ToList()
            };
            return await Task.FromResult(View("~/Features/Blocks/CategoryBlock/CategoryBlock.cshtml", model));
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