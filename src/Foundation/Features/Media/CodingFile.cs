using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Coding File", GUID = "cbbfab00-eac0-40ab-b9bf-2966b901841e", Description = "Used for coding file types such as Css, Javascript.")]
    [MediaDescriptor(ExtensionString = "css,js")]
    public class CodingFile : MediaData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Description { get; set; }
    }
}