using EPiServer.Core;
using EPiServer.Web;
using Foundation.Demo.Models;
using System.Collections.Generic;
using System.Globalization;

namespace Foundation.Demo.ViewModels
{
    public class DemoFooterViewModel
    {
        public virtual DemoHomePage HomePage { get; set; }
        public virtual PageData CurrentPage { get; set; }
        public virtual List<SiteDefinition> SiteDefinitions { get; set; }
        public virtual IEnumerable<KeyValuePair<CultureInfo, string>> CurrentPageLanguages { get; set; }

        public DemoFooterViewModel(PageData currentPage)
        {
            CurrentPage = currentPage;
            SiteDefinitions = new List<SiteDefinition>();
            CurrentPageLanguages = new List<KeyValuePair<CultureInfo, string>>();
        }
    }
}
