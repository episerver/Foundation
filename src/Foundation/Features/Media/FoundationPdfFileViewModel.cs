namespace Foundation.Features.Media
{
    public class FoundationPdfFileViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowIcon { get; set; }
        public int Id { get; set; }
        public int Height { get; set; }
        public string PdfLink { get; set; }
        public bool DisplayAsPreview { get; set; }
    }
}
