using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Cms.SiteSettings.Models;
using Foundation.Cms.SiteSettings.Pages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    public abstract class CmsHomePage : FoundationPageData
    {
        #region Content

        [CultureSpecific]
        [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 190)]
        public virtual ContentArea TopContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Bottom content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea BottomContentArea { get; set; }

        #endregion

        [CultureSpecific]
        [Display(Name = "Site settings node", GroupName = SystemTabNames.Content, Order = 500)]
        [AllowedTypes(AllowedTypes = new[] { typeof(SettingNode) })]
        public virtual ContentReference SettingNode { get; set; }

        #region Settings properties
        private CmsSettingsModel CmsSiteSettings {
            get
            {
                var siteSettingsProvider = ServiceLocator.Current.GetInstance<ISiteSettingsProvider>();
                var settings = siteSettingsProvider.GetSiteSettings<CmsSettingsModel>(this.SettingNode);
                return settings;
            }
        }
        public string Introduction => CmsSiteSettings.LayoutSettings.Introduction;
        public string CompanyHeader => CmsSiteSettings.LayoutSettings.CompanyHeader;
        public string CompanyName => CmsSiteSettings.LayoutSettings.CompanyName;
        public string CompanyAddress => CmsSiteSettings.LayoutSettings.CompanyAddress;
        public string CompanyPhone => CmsSiteSettings.LayoutSettings.CompanyPhone;
        public string CompanyEmail => CmsSiteSettings.LayoutSettings.CompanyEmail;
        public string LinksHeader => CmsSiteSettings.LayoutSettings.LinksHeader;
        public LinkItemCollection Links => CmsSiteSettings.LayoutSettings.Links;
        public string SocialHeader => CmsSiteSettings.LayoutSettings.SocialHeader;
        public LinkItemCollection SocialLinks => CmsSiteSettings.LayoutSettings.SocialLinks;
        public ContentArea ContentArea => CmsSiteSettings.LayoutSettings.ContentArea;
        public string FooterCopyrightText => CmsSiteSettings.LayoutSettings.FooterCopyrightText;
        public ContentArea MainMenu => CmsSiteSettings.LayoutSettings.MainMenu;
        public LinkItemCollection MyAccountCmsMenu => CmsSiteSettings.LayoutSettings.MyAccountCmsMenu;
        public IList<SelectionItem> Sectors => CmsSiteSettings.SelectionSettings.Sectors;
        public IList<SelectionItem> Locations => CmsSiteSettings.SelectionSettings.Locations;
        public IList<ColorModel> ColorOptions => CmsSiteSettings.SelectionSettings.ColorOptions;
        #endregion
    }
}