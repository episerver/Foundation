using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Media
{
    [ContentType(GUID = "58341C80-E78F-4F83-AF11-3B48563B41CA")]
    [MediaDescriptor(ExtensionString = "pdf")]
    public class PDFFile : MediaData
    {
        ///// <summary>
        ///// Gets or sets the description.
        ///// </summary>
        //public virtual String Title { get; set; }

        [Editable(false)]
        public virtual string FileSize { get; set; }
    }

}


