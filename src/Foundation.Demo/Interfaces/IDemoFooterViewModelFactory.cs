using EPiServer.Core;
using Foundation.Demo.Models;
using Foundation.Demo.ViewModels;
using System.Collections.Generic;
using System.Globalization;

namespace Foundation.Demo.Interfaces
{
    public interface IDemoFooterViewModelFactory
    {
        IEnumerable<KeyValuePair<CultureInfo, string>> GetCurrentPageLanguages(PageData currentPage);
        DemoFooterViewModel CreateDemoFooterViewModel(DemoHomePage homePage, PageData currentPage);
    }
}
