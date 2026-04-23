namespace Foundation.Features.Blocks.AssetsDownloadLinksBlock.Component
{
    public class AssetsDownloadLinksBlockComponent : AsyncBlockComponent<AssetsDownloadLinksBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public AssetsDownloadLinksBlockComponent(IContentLoader contentLoader, UrlResolver urlResolver)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(AssetsDownloadLinksBlock currentBlock)
        {
            var model = new AssetsDownloadLinksBlockViewModel(currentBlock);
            var rootContent = _contentLoader.Get<IContent>(currentBlock.RootContent);
            if (rootContent != null)
            {
                var assets = new List<MediaData>();
                if (rootContent is ContentFolder)
                {
                    assets = _contentLoader.GetChildren<MediaData>(rootContent.ContentLink).OrderByDescending(x => x.StartPublish).ToList();
                }

                if (rootContent is IAssetContainer assetContainer)
                {
                    assets = assetContainer.GetAssetsMediaData(_contentLoader, currentBlock.GroupName)
                        .OrderByDescending(x => x.StartPublish).ToList();
                }

                if (currentBlock.Count > 0)
                {
                    assets = assets.Take(currentBlock.Count).ToList();
                }

                model.Assets = assets;
            }

            return await Task.FromResult(View("~/Features/Blocks/AssetsDownloadLinksBlock/AssetsDownloadLinksBlock.cshtml", model));
        }
    }
}
