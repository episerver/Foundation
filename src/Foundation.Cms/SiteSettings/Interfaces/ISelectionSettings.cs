using Foundation.Cms.SiteSettings.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.SiteSettings.Interfaces
{
    public interface ISelectionSettings
    {
        IList<SelectionItem> Sectors { get; set; }
        IList<SelectionItem> Locations { get; set; }
        IList<ColorModel> ColorOptions { get; set; }
    }
}
