using EPiServer.Data.Dynamic;

namespace Foundation.Features.Blog.BlogItemPage
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class BlogTagItem : IDynamicData
    {
        public string TagName { get; set; }
        public int Count { get; set; }
        public int Weight { get; set; }
        public string Url { get; set; }
        public string DisplayName { get; set; }
        public EPiServer.Data.Identity Id { get; set; }
    }
}