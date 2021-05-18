using EPiServer.Core;
using EPiServer.DataAnnotations;
using System;

namespace Foundation.Test
{
    [ContentType(GUID = "14bbf4a1-cd38-47f0-a550-1028cc989c4f",
        AvailableInEditMode = true)]
    public class Comment : ContentData, IContent
    {
        public virtual XhtmlString UserComment { get; set; }

        public virtual string PostedBy { get; set; }

        public string Name { get; set; }

        public ContentReference ContentLink { get; set; }

        public ContentReference ParentLink { get; set; }

        public Guid ContentGuid { get; set; }

        public int ContentTypeID { get; set; }

        public bool IsDeleted { get; set; }
    }
}