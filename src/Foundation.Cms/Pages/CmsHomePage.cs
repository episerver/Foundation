using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using Foundation.Cms.SiteSettings;
using Foundation.Cms.ViewModels;
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
        [Display(Name = "Settings page", GroupName = "Site structure", Order = 100)]
        [AllowedTypes(new[] { typeof(CmsSettingsPage) })]
        public virtual ContentReference SettingsPage { get; set; }


        // Get settings
        public virtual CmsSiteSettings SiteSettings {
            get
            {
                var _siteSettingsProvider = ServiceLocator.Current.GetInstance<ISiteSettingsProvider>();
                return _siteSettingsProvider.GetSiteSettings<CmsSiteSettings>(this);
            }
        }

        public virtual ContentArea MainMenu => SiteSettings.MainMenu;
        public virtual LinkItemCollection MyAccountCmsMenu => SiteSettings.MyAccountCmsMenu;
        public virtual string Introduction  => SiteSettings.Introduction;
        public virtual string CompanyHeader  => SiteSettings.CompanyHeader;
        public virtual string CompanyName  => SiteSettings.CompanyName;
        public virtual string CompanyAddress  => SiteSettings.CompanyAddress;
        public virtual string CompanyPhone  => SiteSettings.CompanyPhone;
        public virtual string CompanyEmail  => SiteSettings.CompanyEmail;
        public virtual string LinksHeader  => SiteSettings.LinksHeader;
        public virtual LinkItemCollection Links  => SiteSettings.Links;
        public virtual string SocialHeader  => SiteSettings.SocialHeader;
        public virtual LinkItemCollection SocialLinks  => SiteSettings.SocialLinks;
        public virtual ContentArea ContentArea  => SiteSettings.ContentArea;
        public virtual string FooterCopyrightText  => SiteSettings.FooterCopyrightText;
    }
}