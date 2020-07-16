using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Cms;
using Foundation.Commerce.Models.EditorDescriptors;
using Foundation.Features.Blocks.MenuItemBlock;
using Foundation.Features.Checkout;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.OrderConfirmation;
using Foundation.Features.MyAccount.OrderDetails;
using Foundation.Features.MyAccount.OrderHistory;
using Foundation.Features.MyAccount.ResetPassword;
using Foundation.Features.MyAccount.SubscriptionDetail;
using Foundation.Features.MyAccount.SubscriptionHistory;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Features.MyOrganization.QuickOrderPage;
using Foundation.Features.MyOrganization.SubOrganization;
using Foundation.Features.NamedCarts.DefaultCart;
using Foundation.Features.NamedCarts.OrderPadsPage;
using Foundation.Features.NamedCarts.SharedCart;
using Foundation.Features.NamedCarts.Wishlist;
using Foundation.Features.Search.Search;
using Foundation.Features.Shared;
using Foundation.Find.Facets;
using Foundation.Find.Facets.Config;
using Foundation.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Home
{
    [ContentType(DisplayName = "Home Page",
        GUID = "452d1812-7385-42c3-8073-c1b7481e7b20",
        Description = "Used for home page of all sites",
        AvailableInEditMode = true,
        GroupName = GroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-02.png")]
    public class HomePage : FoundationPageData, IFacetConfiguration
    {
        #region Header

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Site logo", GroupName = TabNames.Header, Order = 10)]
        public virtual ContentReference SiteLogo { get; set; }

        [SelectOne(SelectionFactoryType = typeof(HeaderMenuSelectionFactory))]
        [Display(Name = "Menu style", GroupName = TabNames.Header, Order = 30)]
        public virtual string HeaderMenuStyle { get; set; }

        [Display(Name = "Large header menu", GroupName = TabNames.Header, Order = 35)]
        public virtual bool LargeHeaderMenu { get; set; }

        [Display(Name = "Show commerce header components", GroupName = TabNames.Header, Order = 40)]
        public virtual bool ShowCommerceHeaderComponents { get; set; }

        [Display(Name = "Sticky header", GroupName = TabNames.Header, Order = 50)]
        public virtual bool StickyTopHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Banner text", GroupName = TabNames.Header, Order = 20)]
        public virtual XhtmlString BannerText { get; set; }

        #endregion

        #region Search Settings

        [SelectOne(SelectionFactoryType = typeof(SearchOptionSelectionFactory))]
        [Display(Name = "Search option", GroupName = TabNames.SearchSettings, Order = 50)]
        public virtual string SearchOption { get; set; }

        [Display(Name = "Show products in search results", GroupName = TabNames.SearchSettings, Order = 100)]
        public virtual bool ShowProductSearchResults { get; set; }

        [Display(Name = "Show contents in search results", GroupName = TabNames.SearchSettings, Order = 150)]
        public virtual bool ShowContentSearchResults { get; set; }

        [Display(Name = "Show PDFs in search results", GroupName = TabNames.SearchSettings, Order = 175)]
        public virtual bool ShowPdfSearchResults { get; set; }

        [Display(Name = "Include images in contents search results", GroupName = TabNames.SearchSettings, Order = 200)]
        public virtual bool IncludeImagesInContentsSearchResults { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CatalogSelectionFactory))]
        [Display(Name = "Search catalog", GroupName = TabNames.SearchSettings, Order = 250,
            Description = "The catalogs that will be returned by search.")]
        public virtual int SearchCatalog { get; set; }

        [Display(
          Name = "Search Filters Configuration",
          Description = "Manage filters to be displayed on Search",
          GroupName = TabNames.SearchSettings,
          Order = 300)]
        [EditorDescriptor(EditorDescriptorType = typeof(IgnoreCollectionEditorDescriptor<FacetFilterConfigurationItem>))]
        public virtual IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }

        #endregion

        #region Menu
        [CultureSpecific]
        [Display(Name = "My account menu (commerce)",
            Description = "This menu will show if show commerce components in header is true.",
            GroupName = TabNames.Menu,
            Order = 30)]
        public virtual LinkItemCollection MyAccountCommerceMenu { get; set; }

        [CultureSpecific]
        [Display(Name = "Organization menu", GroupName = TabNames.Menu, Order = 50)]
        public virtual LinkItemCollection OrganizationMenu { get; set; }

        #endregion

        #region Site Labels

        [Display(Name = "My account", GroupName = TabNames.SiteLabels, Order = 10)]
        public virtual string MyAccountLabel { get; set; }

        [Display(Name = "Shopping cart", GroupName = TabNames.SiteLabels, Order = 20)]
        public virtual string CartLabel { get; set; }

        [Display(Name = "Search", GroupName = TabNames.SiteLabels, Order = 30)]
        public virtual string SearchLabel { get; set; }

        [Display(Name = "Wishlist", GroupName = TabNames.SiteLabels, Order = 40)]
        public virtual string WishlistLabel { get; set; }

        [Display(Name = "Shared cart", GroupName = TabNames.SiteLabels, Order = 50)]
        public virtual string SharedCartLabel { get; set; }

        #endregion

        #region Site Structure

        [Display(Name = "Show product ratings on all product tiles", GroupName = TabNames.SiteStructure, Order = 5)]
        public virtual bool ShowProductRatingsOnListings { get; set; }

        [AllowedTypes(typeof(SearchResultPage))]
        [Display(Name = "Search page", GroupName = TabNames.SiteStructure, Order = 10)]
        public virtual ContentReference SearchPage { get; set; }

        [Display(Name = "Store locator page", GroupName = TabNames.SiteStructure, Order = 20)]
        public virtual ContentReference StoreLocatorPage { get; set; }

        [AllowedTypes(typeof(AddressBookPage))]
        [Display(Name = "Address book page", GroupName = TabNames.SiteStructure, Order = 30)]
        public virtual ContentReference AddressBookPage { get; set; }

        [AllowedTypes(typeof(ResetPasswordPage))]
        [Display(Name = "Reset password page", GroupName = TabNames.SiteStructure, Order = 40)]
        public virtual ContentReference ResetPasswordPage { get; set; }

        [AllowedTypes(typeof(WishListPage))]
        [Display(Name = "Wishlist page", GroupName = TabNames.SiteStructure, Order = 50)]
        public virtual ContentReference WishlistPage { get; set; }

        [AllowedTypes(typeof(CartPage))]
        [Display(Name = "Shopping cart page", GroupName = TabNames.SiteStructure, Order = 60)]
        public virtual ContentReference CartPage { get; set; }

        [AllowedTypes(typeof(SharedCartPage))]
        [Display(Name = "Shared cart page", GroupName = TabNames.SiteStructure, Order = 70)]
        public virtual ContentReference SharedCartPage { get; set; }

        [AllowedTypes(typeof(SubscriptionDetailPage))]
        [Display(Name = "Payment plan details page", GroupName = TabNames.SiteStructure, Order = 80)]
        public virtual ContentReference PaymentPlanDetailsPage { get; set; }

        [AllowedTypes(typeof(SubscriptionHistoryPage))]
        [Display(Name = "Payment plan history page", GroupName = TabNames.SiteStructure, Order = 90)]
        public virtual ContentReference PaymentPlanHistoryPage { get; set; }

        [AllowedTypes(typeof(OrganizationPage))]
        [Display(Name = "Organization main page", GroupName = TabNames.SiteStructure, Order = 100)]
        public virtual ContentReference OrganizationMainPage { get; set; }

        [AllowedTypes(typeof(SubOrganizationPage))]
        [Display(Name = "Sub-organization page", GroupName = TabNames.SiteStructure, Order = 110)]
        public virtual ContentReference SubOrganizationPage { get; set; }

        [Display(Name = "Organization order pads page", GroupName = TabNames.SiteStructure, Order = 120)]
        [AllowedTypes(typeof(OrderPadsPage))]
        public virtual ContentReference OrganizationOrderPadsPage { get; set; }

        [AllowedTypes(typeof(QuickOrderPage))]
        [Display(Name = "Quick order page", GroupName = TabNames.SiteStructure, Order = 130)]
        public virtual ContentReference QuickOrderPage { get; set; }

        [AllowedTypes(typeof(OrderDetailsPage))]
        [Display(Name = "Order details page", GroupName = TabNames.SiteStructure, Order = 140)]
        public virtual ContentReference OrderDetailsPage { get; set; }

        [AllowedTypes(typeof(OrderHistoryPage))]
        [Display(Name = "Order history page", GroupName = TabNames.SiteStructure, Order = 150)]
        public virtual ContentReference OrderHistoryPage { get; set; }

        [AllowedTypes(typeof(OrderConfirmationPage))]
        [Display(Name = "Order confirmation page", GroupName = TabNames.SiteStructure, Order = 160)]
        public virtual ContentReference OrderConfirmationPage { get; set; }

        [AllowedTypes(typeof(CheckoutPage))]
        [Display(Name = "Checkout page", GroupName = TabNames.SiteStructure, Order = 170)]
        public virtual ContentReference CheckoutPage { get; set; }

        [Display(Name = "Resource not found page", GroupName = TabNames.SiteStructure, Order = 180)]
        public virtual ContentReference PageNotFound { get; set; }

        #endregion

        #region Mail templates

        [Display(Name = "Send order confirmations", GroupName = TabNames.MailTemplates, Order = 10)]
        public virtual bool SendOrderConfirmationMail { get; set; }

        [AllowedTypes(typeof(MailBasePage))]
        [Display(Name = "Order confirmation", GroupName = TabNames.MailTemplates, Order = 20)]
        public virtual ContentReference OrderConfirmationMail { get; set; }

        [AllowedTypes(typeof(MailBasePage))]
        [Display(Name = "Reset password", GroupName = TabNames.MailTemplates, Order = 30)]
        public virtual ContentReference ResetPasswordMail { get; set; }

        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            LargeHeaderMenu = false;
            SearchCatalog = 0;
        }
        #region Content

        [CultureSpecific]
        [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 190)]
        public virtual ContentArea TopContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Bottom content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea BottomContentArea { get; set; }

        #endregion

        #region Menu   

        [AllowedTypes(new[] { typeof(MenuItemBlock), typeof(PageData) })]
        [UIHint("HideContentAreaActionsContainer", PresentationLayer.Edit)]
        [Display(Name = "Main menu", GroupName = TabNames.Menu, Order = 10)]
        public virtual ContentArea MainMenu { get; set; }

        [CultureSpecific]
        [Display(Name = "My account menu (CMS)",
            Description = "This menu will show if show commerce components in header is false",
            GroupName = TabNames.Menu,
            Order = 40)]
        public virtual LinkItemCollection MyAccountCmsMenu { get; set; }

        #endregion

        #region Footer

        [Display(Name = "Introduction", GroupName = TabNames.Footer, Order = 10)]
        public virtual string Introduction { get; set; }

        [Display(Name = "Company header", GroupName = TabNames.Footer, Order = 20)]
        public virtual string CompanyHeader { get; set; }

        [Display(Name = "Company name", GroupName = TabNames.Footer, Order = 25)]
        public virtual string CompanyName { get; set; }

        [Display(Name = "Company address", GroupName = TabNames.Footer, Order = 30)]
        public virtual string CompanyAddress { get; set; }

        [Display(Name = "Company phone", GroupName = TabNames.Footer, Order = 40)]
        public virtual string CompanyPhone { get; set; }

        [Display(Name = "Company email", GroupName = TabNames.Footer, Order = 50)]
        public virtual string CompanyEmail { get; set; }

        [Display(Name = "Links header", GroupName = TabNames.Footer, Order = 60)]
        public virtual string LinksHeader { get; set; }

        [UIHint("FooterColumnNavigation")]
        [Display(Name = "Links", GroupName = TabNames.Footer, Order = 70)]
        public virtual LinkItemCollection Links { get; set; }

        [Display(Name = "Social header", GroupName = TabNames.Footer, Order = 80)]
        public virtual string SocialHeader { get; set; }

        [Display(Name = "Social links", GroupName = TabNames.Footer, Order = 85)]
        public virtual LinkItemCollection SocialLinks { get; set; }

        [CultureSpecific]
        [Display(Name = "Content area", GroupName = TabNames.Footer, Order = 90)]
        public virtual ContentArea ContentArea { get; set; }

        [Display(Name = "Copyright", GroupName = TabNames.Footer, Order = 130)]
        public virtual string FooterCopyrightText { get; set; }

        #endregion

        [Display(GroupName = TabNames.CustomSettings, Order = 100)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<SelectionItem>))]
        public virtual IList<SelectionItem> Sectors { get; set; }

        [Display(GroupName = TabNames.CustomSettings, Order = 200)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<SelectionItem>))]
        public virtual IList<SelectionItem> Locations { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class SelectionItemProperty : PropertyList<SelectionItem>
    {
    }

    public class HeaderMenuSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem {Text = "Center logo", Value = "CenterLogo"},
                new SelectItem {Text = "Left logo", Value = "LeftLogo"}
            };
        }
    }

    public class SearchOptionSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem {Text = "Quick search", Value = "QuickSearch"},
                new SelectItem {Text = "Auto search", Value = "AutoSearch"}
            };
        }
    }
}