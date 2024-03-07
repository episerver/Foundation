using EPiServer.Framework.DataAnnotations;
using EPiServer.PdfPreview.Models;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Pdf File", GUID = "ee7e1eb6-2b6d-4cc9-8ed1-56ec0cbaa40b", Description = "Used for PDF file")]
    [MediaDescriptor(ExtensionString = "pdf")]
    public class FoundationPdfFile : PdfFile
    {
        [Display(Name = "Display as Preview?", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual bool DisplayAsPreview { get; set; }

        [Display(Name = "Show Description (non-Preview Mode)?", 
            GroupName = SystemTabNames.Content, Order = 20)]
        public virtual bool ShowDescription { get; set; }

        [Display(Name = "Show Icon (non-Preview Mode)?", GroupName = SystemTabNames.Content, Order = 25)]
        public virtual bool ShowIcon { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual string Description { get; set; }

        [Display(
            Name = "Preview Height",
            Description = "The height of PDF preview embed (px)",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual int Height { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Height = 500; 
        }
    }
}