using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Cms.SiteSettings.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.SiteSettings.Models
{
    public class SelectionSettingsModel : ISelectionSettings
    {
        public IList<SelectionItem> Sectors { get; set; }
        public IList<SelectionItem> Locations { get; set; }
        public IList<ColorModel> ColorOptions { get; set; }
    }
}
