using EPiServer.Data.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Blog.BlogItemPage
{
    public class BlogTagRepository
    {
        private static BlogTagRepository _instance;
        private static DynamicDataStore Store => typeof(BlogTagItem).GetStore();

        public static BlogTagRepository Instance => _instance ?? (_instance = new BlogTagRepository());

        public void SaveTags(IEnumerable<BlogTagItem> tags)
        {
            foreach (var item in tags)
            {
                SaveTag(item);
            }
        }

        public bool SaveTag(BlogTagItem tag)
        {
            try
            {
                var currentTags = LoadTag(tag);
                if (currentTags == null)
                {
                    currentTags = tag;
                }
                else
                {
                    currentTags.TagName = tag.TagName;
                    currentTags.Count = tag.Count;
                    currentTags.Weight = tag.Weight;
                    currentTags.DisplayName = tag.DisplayName;
                }
                Store.Save(currentTags);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void IncreaseTagCount(BlogTagItem tag)
        {
            tag.Count++;

            SaveTag(tag);
        }

        public IEnumerable<BlogTagItem> LoadTags()
        {
            var list = Store.Items<BlogTagItem>();
            if (list != null)
            {
                return list;
            }
            return new List<BlogTagItem>();
        }

        public BlogTagItem LoadTag(BlogTagItem tag) => LoadTags().FirstOrDefault(x => x.TagName == tag.TagName);
    }
}