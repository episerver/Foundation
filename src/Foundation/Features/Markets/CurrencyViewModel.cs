using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Foundation.Features.Markets
{
    public class CurrencyViewModel
    {
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public string CurrencyCode { get; set; }
    }
}