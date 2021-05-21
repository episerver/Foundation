﻿using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Labs.ContentManager.Dashboard;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Features.Shared;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Features.CatalogContent.Package
{
    [CatalogContentType(DisplayName = "Generic Package", GUID = "7b18ab7a-6344-4879-928e-e1b129d7379c", Description = "")]
    public class GenericPackage : PackageContent, IProductRecommendations, IFoundationContent, IDashboardItem
    {
        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Description", Order = 5)]
        public virtual XhtmlString Description { get; set; }

        [Display(Name = "On sale", Order = 10)]
        public virtual bool OnSale { get; set; }

        [Display(Name = "New arrival", Order = 15)]
        public virtual bool NewArrival { get; set; }

        [Searchable]
        [CultureSpecific]
        [Tokenize]
        [IncludeInDefaultSearch]
        [Display(Name = "Long description", Order = 20)]
        public virtual XhtmlString LongDescription { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Content area",
            Description = "This will display the content area.",
            Order = 25)]
        public virtual ContentArea ContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Associations title",
            Description = "This is title of the Associations tab.",
            Order = 30)]
        public virtual string AssociationsTitle { get; set; }

        [Display(Name = "Show recommendations", Order = 35)]
        public virtual bool ShowRecommendations { get; set; }

        #region Implement IFoundationContent

        [Display(Name = "Hide site header", GroupName = Infrastructure.TabNames.Settings, Order = 100)]
        public virtual bool HideSiteHeader { get; set; }

        [Display(Name = "Hide site footer", GroupName = Infrastructure.TabNames.Settings, Order = 200)]
        public virtual bool HideSiteFooter { get; set; }

        [Display(Name = "CSS files", GroupName = Infrastructure.TabNames.Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Searchable(false)]
        [Display(Name = "CSS", GroupName = Infrastructure.TabNames.Styles, Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }

        [Display(Name = "Script files", GroupName = Infrastructure.TabNames.Scripts, Order = 100)]
        public virtual LinkItemCollection ScriptFiles { get; set; }

        [Searchable(false)]
        [UIHint(UIHint.Textarea)]
        [Display(GroupName = Infrastructure.TabNames.Scripts, Order = 200)]
        public virtual string Scripts { get; set; }

        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            AssociationsTitle = "You May Also Like";
        }

        public void SetItem(ItemModel itemModel)
        {
            itemModel.Description = Description?.ToHtmlString();
            itemModel.Image = CommerceMediaCollection.FirstOrDefault()?.AssetLink;
        }
    }
}