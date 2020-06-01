using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Foundation.Cms;
using Foundation.Cms.SiteSettings.Pages;
using Foundation.Commerce.SiteSettings.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.SiteSettings.Pages
{
    [ContentType(DisplayName = "Commerce Layout Settings Page",
        GUID = "7c97b9e5-dce3-4e7c-a4a2-d3b582b129a4", 
        Description = "Header settings, footer settings, menu settings, label settings",
        AvailableInEditMode = false,
        GroupName = CmsGroupNames.Settings)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-layout-settings.png")]
    public class CommerceLayoutSettingsPage : CmsLayoutSettingsPage, ICommerceLayoutSettings
    {
        #region Header

        [CultureSpecific]
        [Display(Name = "Banner text", GroupName = CmsTabNames.Header, Order = 20)]
        public virtual XhtmlString BannerText { get; set; }

        #endregion

        #region Menu
        [CultureSpecific]
        [Display(Name = "My account menu (commerce)",
            Description = "This menu will show if show commerce components in header is true.",
            GroupName = CmsTabNames.Menu,
            Order = 30)]
        public virtual LinkItemCollection MyAccountCommerceMenu { get; set; }

        [CultureSpecific]
        [Display(Name = "Organization menu", GroupName = CmsTabNames.Menu, Order = 50)]
        public virtual LinkItemCollection OrganizationMenu { get; set; }

        #endregion

        #region Site Labels

        [Display(Name = "My account", GroupName = CommerceTabNames.SiteLabels, Order = 10)]
        public virtual string MyAccountLabel { get; set; }

        [Display(Name = "Shopping cart", GroupName = CommerceTabNames.SiteLabels, Order = 20)]
        public virtual string CartLabel { get; set; }

        [Display(Name = "Search", GroupName = CommerceTabNames.SiteLabels, Order = 30)]
        public virtual string SearchLabel { get; set; }

        [Display(Name = "Wishlist", GroupName = CommerceTabNames.SiteLabels, Order = 40)]
        public virtual string WishlistLabel { get; set; }

        [Display(Name = "Shared cart", GroupName = CommerceTabNames.SiteLabels, Order = 50)]
        public virtual string SharedCartLabel { get; set; }

        #endregion

    }
}