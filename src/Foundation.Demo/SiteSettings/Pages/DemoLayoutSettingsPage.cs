using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms;
using Foundation.Cms.EditorDescriptors;
using Foundation.Commerce.SiteSettings.Pages;
using Foundation.Demo.SiteSettings.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Demo.SiteSettings.Pages
{
    [ContentType(DisplayName = "Demo Layout Settings Page", 
        GUID = "67df0504-a5e7-40c2-9d69-4a8a6bbc9c13", 
        Description = "Header settings, footer settings, menu settings, label settings",
        GroupName = CmsGroupNames.Settings)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-layout-settings.png")]
    public class DemoLayoutSettingsPage : CommerceLayoutSettingsPage, IDemoLayoutSettings
    {
        #region Header

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Site logo", GroupName = CmsTabNames.Header, Order = 10)]
        public virtual ContentReference SiteLogo { get; set; }

        [SelectOne(SelectionFactoryType = typeof(HeaderMenuSelectionFactory))]
        [Display(Name = "Menu style", GroupName = CmsTabNames.Header, Order = 30)]
        public virtual string HeaderMenuStyle { get; set; }

        [Display(Name = "Large header menu", GroupName = CmsTabNames.Header, Order = 35)]
        public virtual bool LargeHeaderMenu { get; set; }

        [Display(Name = "Show commerce header components", GroupName = CmsTabNames.Header, Order = 40)]
        public virtual bool ShowCommerceHeaderComponents { get; set; }

        [Display(Name = "Sticky header", GroupName = CmsTabNames.Header, Order = 50)]
        public virtual bool StickyTopHeader { get; set; }

        #endregion
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            LargeHeaderMenu = false;
        }
    }
}