using EPiServer.Framework.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Standard File", GUID = "646ECE50-3CE7-4F8B-BA33-9924C9ADC9C6", Description = "Used for standard file types such as Word, Excel, PowerPoint or text documents.")]
    [MediaDescriptor(ExtensionString = "txt,doc,docx,xls,xlsx,ppt,pptx")]
    public class StandardFile : MediaData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Description { get; set; }

        [Display(Name = "Show Description?", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual bool ShowDescription { get; set; }

        [Display(Name = "Show Icon?", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual bool ShowIcon { get; set; }

        [Editable(false)]
        public virtual string FileSize { get; set; }
    }
}
