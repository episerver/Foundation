using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Foundation.Commerce.Models.Blocks
{
    [ContentType(DisplayName = "Media Block",
        GUID = "1bc63398-0708-4bfb-a873-13ddce1c2ab4",
        Description = "Media block")]
    public class MediaBlock : BlockData
    {
        public virtual ContentReference Image { get; set; }

        public virtual string Name { get; set; }

        public virtual XhtmlString Description { get; set; }
    }
}