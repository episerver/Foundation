using EPiServer.DataAnnotations;
using Foundation.Cms.Settings;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Label Settings",
        GUID = "c17375a6-4a01-402b-8c7f-18257e944527",
        SettingsName = "Site Labels")]
    public class LabelSettings : SettingsBase
    {
        [CultureSpecific]
        [Display(Name = "My account", GroupName = TabNames.SiteLabels, Order = 10)]
        public virtual string MyAccountLabel { get; set; }

        [CultureSpecific]
        [Display(Name = "Shopping cart", GroupName = TabNames.SiteLabels, Order = 20)]
        public virtual string CartLabel { get; set; }

        [CultureSpecific]
        [Display(Name = "Search", GroupName = TabNames.SiteLabels, Order = 30)]
        public virtual string SearchLabel { get; set; }

        [CultureSpecific]
        [Display(Name = "Wishlist", GroupName = TabNames.SiteLabels, Order = 40)]
        public virtual string WishlistLabel { get; set; }

        [CultureSpecific]
        [Display(Name = "Shared cart", GroupName = TabNames.SiteLabels, Order = 50)]
        public virtual string SharedCartLabel { get; set; }
    }
}