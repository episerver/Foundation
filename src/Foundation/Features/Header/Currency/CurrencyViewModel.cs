using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Header.Currency
{
    public class CurrencyViewModel
    {
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public string CurrencyCode { get; set; }
    }
}