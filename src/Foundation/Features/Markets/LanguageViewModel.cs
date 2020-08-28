using EPiServer.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Markets
{
    public class LanguageViewModel
    {
        public IEnumerable<SelectListItem> Languages { get; set; }
        public string Language { get; set; }
        public ContentReference ContentLink { get; set; }
    }
}