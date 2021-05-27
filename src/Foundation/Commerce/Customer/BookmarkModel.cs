using EPiServer.Core;
using System;

namespace Foundation.Commerce.Customer
{
    public class BookmarkModel
    {
        public ContentReference ContentLink { get; set; }
        public Guid ContentGuid { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
