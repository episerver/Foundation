using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    public abstract class CmsHomePage : FoundationPageData
    {
        #region Content

        [CultureSpecific]
        [Display(
            Name = "Top content area",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 4)]
        public virtual ContentArea TopContentArea { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Bottom content area",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 6)]
        public virtual ContentArea BottomContentArea { get; set; }

        #endregion

        #region Menu   
        [Display(Name = "Mobile Navigation", GroupName = CmsTabs.Menu, Order = 3)]
        public virtual LinkItemCollection MobileNavigationPages { get; set; }

        [CultureSpecific]
        [Display(
            Name = "My Account Cms Navigation",
            Description = "This menu will show if show commerce components in header is false",
            GroupName = CmsTabs.Menu,
            Order = 4)]
        public virtual LinkItemCollection MyAccountCmsMenu { get; set; }

        [Display(Name = "Main Menu", GroupName = CmsTabs.Menu, Order = 1)]
        [UIHint("HideContentAreaActionsContainer", PresentationLayer.Edit)]
        [AllowedTypes(new[] { typeof(MenuItemBlock) })]
        public virtual ContentArea MainMenu { get; set; }

        #endregion

        #region Footer

        [Display(Name = "Footer Company Header", GroupName = CmsTabs.Footer, Order = 30)]
        public virtual string FooterCompanyHeader { get; set; }

        [Display(Name = "Footer Follow Social Header", GroupName = CmsTabs.Footer, Order = 50)]
        public virtual string FooterFollowHeader { get; set; }

        [Display(Name = "Footer Links Header", GroupName = CmsTabs.Footer, Order = 70)]
        public virtual string FooterLinksHeader { get; set; }

        [Display(Name = "Footer Links", GroupName = CmsTabs.Footer, Order = 80)]
        [UIHint("FooterColumnNavigation")]
        public virtual LinkItemCollection FooterColumnLinks { get; set; }

        [Display(Name = "Footer Comapny Address", GroupName = CmsTabs.Footer, Order = 100)]
        public virtual string FooterCompanyAddress { get; set; }

        [Display(Name = "Footer Comapny Phone", GroupName = CmsTabs.Footer, Order = 110)]
        public virtual string FooterCompanyPhone { get; set; }

        [Display(Name = "Footer Company Email", GroupName = CmsTabs.Footer, Order = 120)]
        public virtual string FooterCompanyEmail { get; set; }

        [Display(Name = "Footer Copyright", GroupName = CmsTabs.Footer, Order = 130)]
        public virtual string FooterCopyrightText { get; set; }

        [Display(Name = "Footer Introduction", GroupName = CmsTabs.Footer, Order = 140)]
        public virtual string FooterIntroduction { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Footer content area",
            Description = "",
            GroupName = CmsTabs.Footer,
            Order = 150)]
        public virtual ContentArea FooterContentArea { get; set; }
        #endregion
    }
}