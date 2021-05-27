using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Webp Image File",
        GUID = "46652356-ef68-4ef2-b57e-293aa4f87be8",
        Description = "Used for webp image file type")]
    [MediaDescriptor(ExtensionString = "webp")]
    public class WebImageMediaData : ImageMediaData
    {
        /// <summary>
        /// Gets the generated thumbnail for this media.
        /// </summary>
        public override Blob Thumbnail { get => BinaryData; }
    }
}