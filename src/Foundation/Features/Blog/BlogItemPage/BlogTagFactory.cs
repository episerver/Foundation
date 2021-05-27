using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Blog.BlogItemPage
{
    public class BlogTagFactory
    {
        private readonly IContentRepository _contentRepository;
        private readonly UrlResolver _urlResolver;
        private readonly CategoryRepository _categoryRepository;

        public BlogTagFactory(IContentRepository contentRepository,
            UrlResolver urlResolver,
            CategoryRepository categoryRepository)
        {
            _contentRepository = contentRepository;
            _urlResolver = urlResolver;
            _categoryRepository = categoryRepository;
        }
        public string GetTagUrl(PageData currentPage, ContentReference cat)
        {
            var start = FindRootParentByPageType(currentPage, typeof(BlogListPage.BlogListPage));
            var pageUrl = _urlResolver.GetUrl(start.ContentLink);
            var url = $"{pageUrl}?category={cat.ID}";
            return url;
        }

        protected PageData FindRootParentByPageType(PageData pageData, Type pageType)
        {
            var ancestors = _contentRepository.GetAncestors(pageData.ContentLink);
            var pageTypeId = pageType.GetPageType()?.ID;
            var rootParent = ancestors.Reverse().FirstOrDefault(x => x.ContentTypeID == pageTypeId);
            return rootParent != null ? rootParent as PageData : _contentRepository.Get<PageData>(ContentReference.StartPage);
        }

        public IEnumerable<BlogTagItem> CalculateTags(ContentReference startPoint)
        {
            var blogs = startPoint.FindPagesByPageType(true, typeof(BlogItemPage).GetPageType().ID);

            var tags = new List<BlogTagItem>();

            foreach (var item in blogs)
            {
                foreach (var catID in item.Category)
                {
                    var cat = _categoryRepository.Get(catID);

                    var tagitem = tags.FirstOrDefault(x => x.TagName == cat.Name);

                    if (tagitem == null)
                    {
                        tags.Add(new BlogTagItem()
                        {
                            Count = 1,
                            TagName = cat.Name,
                            DisplayName = cat.Description
                        });
                    }
                    else
                    {
                        tagitem.DisplayName = cat.Description;
                        tagitem.Count++;
                    }
                }
            }

            if (!tags.Any())
            {
                return tags;
            }
            //Now we have all tags and the count, lets find the highest count as well as the lowest count
            var largestCount = 0;
            var smallestCount = 0;

            tags = tags.OrderBy(x => x.Count).ToList();

            smallestCount = tags[0].Count;
            largestCount = tags[tags.Count - 1].Count;

            foreach (var tag in tags)
            {
                var weightPercent = (double.Parse(tag.Count.ToString()) / largestCount) * 100;
                var weight = 0;
                if (weightPercent >= 99)
                {
                    //heaviest
                    weight = 1;
                }
                else if (weightPercent >= 70)
                {
                    weight = 2;
                }
                else if (weightPercent >= 40)
                {
                    weight = 3;
                }
                else if (weightPercent >= 20)
                {
                    weight = 4;
                }
                else if (weightPercent >= 3)
                {
                    //weakest
                    weight = 5;
                }
                else
                {
                    // use this to filter out all low hitters
                    weight = 0;
                }

                tag.Weight = weight;
            }

            return tags;
        }
    }
}