using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Labs.ContentManager.Cards;
using EPiServer.Labs.ContentManager.Dashboard;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Cms.EditorDescriptors;
using Geta.EpiCategories;
using Geta.EpiCategories.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    public abstract class FoundationPageData : PageData, ICategorizableContent, IFoundationContent, IDashboardItem
    {
        #region Page Header

        [Categories]
        [Display(Name = "Categories",
            Description = "Categories associated with this content.",
            GroupName = SystemTabNames.PageHeader,
            Order = 10)]
        public virtual IList<ContentReference> Categories { get; set; }

        #endregion

        #region Content

        [CultureSpecific]
        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 100)]
        public virtual XhtmlString MainBody { get; set; }

        [CultureSpecific]
        [Display(Name = "Main content area", GroupName = SystemTabNames.Content, Order = 200)]
        public virtual ContentArea MainContentArea { get; set; }

        #endregion

        #region Metadata

        [CultureSpecific]
        [Display(Name = "Title", GroupName = CmsTabNames.MetaData, Order = 100)]
        public virtual string MetaTitle
        {
            get
            {
                var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

                return !string.IsNullOrWhiteSpace(metaTitle)
                    ? metaTitle
                    : PageName;
            }
            set => this.SetPropertyValue(p => p.MetaTitle, value);
        }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(GroupName = CmsTabNames.MetaData, Order = 200)]
        public virtual string Keywords { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Page description", GroupName = CmsTabNames.MetaData, Order = 300)]
        public virtual string PageDescription { get; set; }

        [CultureSpecific]
        [Display(Name = "Content type", GroupName = CmsTabNames.MetaData, Order = 310)]
        public virtual string MetaContentType { get; set; }

        [CultureSpecific]
        [Display(Name = "Industry", GroupName = CmsTabNames.MetaData, Order = 320)]
        public virtual string Industry { get; set; }

        [CultureSpecific]
        [Display(Name = "Author", GroupName = CmsTabNames.MetaData, Order = 320)]
        public virtual string AuthorMetaData { get; set; }

        [CultureSpecific]
        [Display(Name = "Disable indexing", GroupName = CmsTabNames.MetaData, Order = 400)]
        public virtual bool DisableIndexing { get; set; }

        #endregion

        #region Settings

        [CultureSpecific]
        [Display(Name = "Exclude from results",
            Description = "This will determine whether or not to show on search",
            GroupName = CmsTabNames.Settings,
            Order = 100)]
        public virtual bool ExcludeFromSearch { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site header", GroupName = CmsTabNames.Settings, Order = 200)]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site footer", GroupName = CmsTabNames.Settings, Order = 300)]
        public virtual bool HideSiteFooter { get; set; }

        [CultureSpecific]
        [Display(Name = "Highlight in page list", GroupName = CmsTabNames.Settings, Order = 400)]
        public virtual bool Highlight { get; set; }

        #endregion

        #region Teaser

        [SelectOne(SelectionFactoryType = typeof(BlockRatioSelectionFactory))]
        [Display(Name = "Teaser ratio (width-height)", GroupName = CmsTabNames.Teaser, Order = 50)]
        public virtual string TeaserRatio { get; set; }

        [UIHint(UIHint.Image)]
        [Display(Name = "Image", GroupName = CmsTabNames.Teaser, Order = 100)]
        public virtual ContentReference PageImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
        [Display(Name = "Video", GroupName = CmsTabNames.Teaser, Order = 200)]
        public virtual ContentReference TeaserVideo { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Text", GroupName = CmsTabNames.Teaser, Order = 300)]
        public virtual string TeaserText
        {
            get
            {
                var teaserText = this.GetPropertyValue(p => p.TeaserText);

                // Use explicitly set teaser text, otherwise fall back to description
                return !string.IsNullOrWhiteSpace(teaserText)
                    ? teaserText
                    : PageDescription;
            }
            set => this.SetPropertyValue(p => p.TeaserText, value);
        }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(TeaserTextAlignmentSelectionFactory))]
        [Display(Name = "Text alignment", GroupName = CmsTabNames.Teaser, Order = 400)]
        public virtual string TeaserTextAlignment { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(TeaserColorThemeSelectionFactory))]
        [Display(Name = "Color theme", GroupName = CmsTabNames.Teaser, Order = 500)]
        public virtual string TeaserColorTheme { get; set; }

        [CultureSpecific]
        [Display(Name = "Button label", GroupName = CmsTabNames.Teaser, Order = 600)]
        public virtual string TeaserButtonText { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(ButtonBlockStyleSelectionFactory))]
        [Display(Name = "Button theme", GroupName = CmsTabNames.Teaser, Order = 700)]
        public virtual string TeaserButtonStyle { get; set; }

        [CultureSpecific]
        [Display(Name = "Display hover effect", GroupName = CmsTabNames.Teaser, Order = 800)]
        public virtual bool ApplyHoverEffect { get; set; }

        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Padding", GroupName = CmsTabNames.Teaser, Order = 900)]
        public virtual string Padding
        {
            get { return this.GetPropertyValue(teaser => teaser.Padding) ?? "p-0"; }
            set { this.SetPropertyValue(teaser => teaser.Padding, value); }
        }

        [SelectOne(SelectionFactoryType = typeof(MarginSelectionFactory))]
        [Display(Name = "Margin", GroupName = CmsTabNames.Teaser, Order = 910)]
        public virtual string Margin
        {
            get { return this.GetPropertyValue(teaser => teaser.Margin) ?? "m-0"; }
            set { this.SetPropertyValue(teaser => teaser.Margin, value); }
        }

        [Ignore]
        public string AlignmentCssClass
        {
            get
            {
                string alignmentClass;
                switch (TeaserTextAlignment)
                {
                    case "Left":
                        alignmentClass = "teaser-content-align--left";
                        break;
                    case "Right":
                        alignmentClass = "teaser-content-align--right";
                        break;
                    case "Center":
                        alignmentClass = "teaser-content-align--center";
                        break;
                    default:
                        alignmentClass = string.Empty;
                        break;
                }

                return alignmentClass;
            }
        }

        [Ignore]
        public string ThemeCssClass
        {
            get
            {
                string themeCssClass;
                switch (TeaserColorTheme)
                {
                    case ColorThemes.Light:
                        themeCssClass = "teaser-theme--light";
                        break;
                    case ColorThemes.Dark:
                        themeCssClass = "teaser-theme--dark";
                        break;
                    default:
                        themeCssClass = null;
                        break;
                }

                return themeCssClass;
            }
        }

        #endregion

        #region Styles

        [Display(Name = "CSS files", GroupName = CmsTabNames.Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Display(Name = "CSS", GroupName = CmsTabNames.Styles, Order = 200)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }

        #endregion

        #region Scripts

        [Display(Name = "Script files", GroupName = CmsTabNames.Scripts, Order = 100)]
        public virtual LinkItemCollection ScriptFiles { get; set; }

        [UIHint(UIHint.Textarea)]
        [Display(GroupName = CmsTabNames.Scripts, Order = 200)]
        public virtual string Scripts { get; set; }

        #endregion

        public virtual void SetItem(ItemModel itemModel)
        {
            itemModel.Description = PageDescription;
            itemModel.Image = PageImage;
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            TeaserTextAlignment = "Left";
            TeaserColorTheme = ColorThemes.Light;
            TeaserRatio = "10-5";
            TeaserButtonStyle = ButtonBlockStyleSelectionFactory.ButtonBlockStyles.TransparentWhite;
            TeaserButtonText = "Read more";
            ApplyHoverEffect = true;
            Padding = "p-1";
            Margin = "m-1";
            base.SetDefaultValues(contentType);
        }
    }
}