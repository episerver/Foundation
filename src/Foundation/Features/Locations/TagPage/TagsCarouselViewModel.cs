using EPiServer.Core;
using System.Collections.Generic;

namespace Foundation.Features.Locations.TagPage
{
    public class TagsCarouselViewModel
    {
        public List<TagsCarouselItem> Items { get; set; }
    }

    public class TagsCarouselItem
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public ContentReference Image { get; set; }
        public ContentReference ItemURL { get; set; }
    }
}