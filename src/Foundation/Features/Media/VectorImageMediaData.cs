using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Vector Image File",
        GUID = "3bedeaa0-67ba-4f6a-a420-dabf6ad6890b",
        Description = "Used for svg image file type")]
    [MediaDescriptor(ExtensionString = "svg")]
    public class VectorImageMediaData : ImageMediaData
    {
        /// <summary>
        /// Gets the generated thumbnail for this media.
        /// </summary>
        public override Blob Thumbnail { get => BinaryData; }
    }
}