using EPiServer.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Foundation.Features.Markets
{
    public class LanguageViewModel
    {
        public IEnumerable<SelectListItem> Languages { get; set; }
        public string Language { get; set; }
        public ContentReference ContentLink { get; set; }
    }
}