using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Commerce.Markets.ViewModels
{
    public class CurrencyViewModel
    {
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public string CurrencyCode { get; set; }
    }
}