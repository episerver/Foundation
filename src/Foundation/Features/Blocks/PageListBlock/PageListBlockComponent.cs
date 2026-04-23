using EPiServer.Filters;
using Foundation.Features.Folder;
using Foundation.Infrastructure.Cms;

namespace Foundation.Features.Blocks.PageListBlock
{
    public class PageListBlockComponent : AsyncBlockComponent<PageListBlock>
    {
        private readonly ContentLocator _contentLocator;
        private readonly IContentLoader _contentLoader;

        public PageListBlockComponent(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            _contentLocator = contentLocator;
            _contentLoader = contentLoader;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(PageListBlock currentBlock)
        {
            var pages = FindPages(currentBlock);
            pages = pages.Where(x => x.PageTypeName != typeof(FolderPage).Name);
            pages = Sort(pages, currentBlock.SortOrder);

            if (currentBlock.Count > 0)
            {
                pages = pages.Take(currentBlock.Count);
            }

            var model = new PageListBlockViewModel(currentBlock)
            {
                Pages = pages.Select(x => new PageListPreviewViewModel(x, currentBlock))
            };

            ViewData.GetEditHints<PageListBlockViewModel, PageListBlock>()
                .AddConnection(x => x.Heading, x => x.Heading);

            await Task.CompletedTask;
            return View("~/Features/Blocks/PageListBlock/Views/PageListBlock.cshtml", model);
        }

        private IEnumerable<PageData> FindPages(PageListBlock currentBlock)
        {
            IEnumerable<PageData> pages = new List<PageData>();
            var current = currentBlock;
            // CMS 13: ContentArea.FilteredItems obsolete. Use Items instead.
            var rootList = currentBlock.Roots?.Items ?? Enumerable.Empty<ContentAreaItem>();
            if (currentBlock.Recursive)
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    foreach (var root in rootList)
                    {
                        // CMS 13: ContentAreaItem.ContentLink is ContentReference; cast to PageReference via ID.
                        var pageRef = ToPageReference(root.ContentLink);
                        if (pageRef == null) continue;
                        var page = _contentLocator.FindPagesByPageType(pageRef, true, currentBlock.PageTypeFilter.ID);
                        pages = pages.Union(page);
                    }
                }
                else
                {
                    foreach (var root in rootList)
                    {
                        if (ContentReference.IsNullOrEmpty(root.ContentLink)) continue;
                        var page = _contentLocator.GetAll<PageData>(root.ContentLink);
                        pages = pages.Union(page);
                    }
                }
            }
            else
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    foreach (var root in rootList)
                    {
                        if (ContentReference.IsNullOrEmpty(root.ContentLink)) continue;
                        var page = _contentLoader.GetChildren<PageData>(root.ContentLink)
                            .Where(p => p.ContentTypeID == currentBlock.PageTypeFilter.ID);
                        pages = pages.Union(page);
                    }
                }
                else
                {
                    foreach (var root in rootList)
                    {
                        if (ContentReference.IsNullOrEmpty(root.ContentLink)) continue;
                        var page = _contentLoader.GetChildren<PageData>(root.ContentLink);
                        pages = pages.Union(page);
                    }
                }
            }
            // Category filtering removed: Geta.Optimizely.Categories has no CMS 13 version.
            pages = pages.Where(x => x.VisibleInMenu);

            return pages;
        }

        // CMS 13: ContentAreaItem.ContentLink is typed as ContentReference, not PageReference.
        // Create a PageReference from the ContentReference ID to pass to APIs that require PageReference.
        private static PageReference ToPageReference(ContentReference contentLink)
        {
            if (ContentReference.IsNullOrEmpty(contentLink)) return null;
            return new PageReference(contentLink.ID, contentLink.WorkID);
        }

        private IEnumerable<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = new PageDataCollection(pages);
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);
            return asCollection;
        }
    }
}
