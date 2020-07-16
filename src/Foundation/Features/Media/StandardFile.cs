using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Standard File", GUID = "646ECE50-3CE7-4F8B-BA33-9924C9ADC9C6", Description = "Used for standard file types such as Word, Excel, PowerPoint or text documents.")]
    [MediaDescriptor(ExtensionString = "txt,doc,docx,xls,xlsx,ppt,pptx")]
    public class StandardFile : MediaData
    {
        [Editable(false)]
        public virtual string FileSize { get; set; }
    }
}
