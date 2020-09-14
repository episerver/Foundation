using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Markets
{
    public class CurrencyViewModel
    {
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public string CurrencyCode { get; set; }
    }
}