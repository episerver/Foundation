using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.PdfPreview.Models;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Pdf File", GUID = "ee7e1eb6-2b6d-4cc9-8ed1-56ec0cbaa40b", Description = "Used for PDF file")]
    [MediaDescriptor(ExtensionString = "pdf")]
    public class FoundationPdfFile : PdfFile
    {
        [Display(
            Name = "Height",
            Description = "The height of PDF preview embed (px)",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual int Height { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Height = 500;
        }
    }
}