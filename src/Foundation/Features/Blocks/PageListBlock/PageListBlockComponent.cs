using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Web.Mvc;
using Foundation.Infrastructure.Cms;
using Foundation.Features.Folder;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public override async Task<IViewComponentResult> InvokeAsync(PageListBlock currentBlock)
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

            return View("~/Features/Blocks/PageListBlock/Views/PageListBlock.cshtml", model);
        }

        private IEnumerable<PageData> FindPages(PageListBlock currentBlock)
        {
            IEnumerable<PageData> pages = new List<PageData>();
            var current = currentBlock;
            var rootList = currentBlock.Roots?.FilteredItems ?? Enumerable.Empty<ContentAreaItem>();
            if (currentBlock.Recursive)
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    foreach (var root in rootList)
                    {
                        var page = _contentLocator.FindPagesByPageType(root.ContentLink as PageReference, true, currentBlock.PageTypeFilter.ID);
                        pages = pages.Union(page);
                    }
                }
                else
                {
                    foreach (var root in rootList)
                    {
                        var page = _contentLocator.GetAll<PageData>(root.ContentLink as PageReference);
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
                        var page = _contentLoader.GetChildren<PageData>(root.ContentLink as PageReference)
                            .Where(p => p.ContentTypeID == currentBlock.PageTypeFilter.ID);
                        pages = pages.Union(page);
                    }
                }
                else
                {
                    foreach (var root in rootList)
                    {
                        var page = _contentLoader.GetChildren<PageData>(root.ContentLink as PageReference);
                        pages = pages.Union(page);
                    }
                }
            }
            if (currentBlock.CategoryListFilter != null && currentBlock.CategoryListFilter.Any())
            {
                //pages = pages.Where(x =>
                //{
                //    var categories = (x as ICategorizableContent)?.Categories;
                //    return categories != null &&
                //           categories.Intersect(currentBlock.CategoryListFilter).Any();
                //});
            }
            pages = pages.Where(x => x.VisibleInMenu);

            return pages;
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
