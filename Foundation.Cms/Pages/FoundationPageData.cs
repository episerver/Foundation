using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Pages
{
    public abstract class FoundationPageData : PageData
    {
        public const string MetaData = CmsTabs.MetaData;
        public const string Styles = CmsTabs.Styles;
        public const string ScriptTag = CmsTabs.Scripts;
        public const string Teaser = CmsTabs.Teaser;

        #region Content

        [Display(GroupName = SystemTabNames.Content, Name = "Main body", Order = 2)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
           GroupName = SystemTabNames.Content,
           Order = 1)]
        [CultureSpecific]
        public virtual ContentArea MainContentArea { get; set; }
        #endregion

        #region Metadata

        [Display(GroupName = MetaData, Order = 100)]
        [CultureSpecific]
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

        [Display(GroupName = MetaData, Order = 200)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string Keyword { get; set; }

        [Display(GroupName = MetaData, Order = 300)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string MetaDescription { get; set; }

        [Display(
            GroupName = MetaData,
            Order = 400)]
        [CultureSpecific]
        public virtual bool DisableIndexing { get; set; }

        #endregion

        #region Teaser

        [Display(GroupName = Teaser, Order = 100)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string TeaserText
        {
            get
            {
                var teaserText = this.GetPropertyValue(p => p.TeaserText);

                // Use explicitly set teaser text, otherwise fall back to description
                return !string.IsNullOrWhiteSpace(teaserText)
                    ? teaserText
                    : MetaDescription;
            }
            set => this.SetPropertyValue(p => p.TeaserText, value);
        }

        [CultureSpecific]
        [Display(Name = "Teaser Text Alignment", GroupName = Teaser, Order = 200)]
        [SelectOne(SelectionFactoryType = typeof(CalloutContentAlignmentSelectionFactory))]
        public virtual string TeaserTextAlignment { get; set; }

        [CultureSpecific]
        [Display(Name = "Teaser Color Theme", GroupName = Teaser, Order = 300)]
        [SelectOne(SelectionFactoryType = typeof(TeaserColorThemeSelectionFactory))]
        public virtual string TeaserColorTheme { get; set; }

        [CultureSpecific]
        [Display(Name = "Teaser Button Text", GroupName = Teaser, Order = 400)]
        public virtual string TeaserButtonText { get; set; }

        [CultureSpecific]
        [Display(Name = "Teaser Button Style", GroupName = Teaser, Order = 410)]
        [SelectOne(SelectionFactoryType = typeof(ButtonBlockStyleSelectionFactory))]
        public virtual string TeaserButtonStyle { get; set; }


        [CultureSpecific]
        [Display(Name = "Teaser Video", GroupName = Teaser, Order = 500)]
        [UIHint(UIHint.Video)]
        public virtual ContentReference TeaserVideo { get; set; }

        [Ignore]
        public string AlignmentCssClass
        {
            get
            {
                string alignmentClass;
                switch (TeaserTextAlignment)
                {
                    case CalloutContentAlignments.Left:
                        alignmentClass = "teaser-content-align--left";
                        break;
                    case CalloutContentAlignments.Right:
                        alignmentClass = "teaser-content-align--right";
                        break;
                    case CalloutContentAlignments.Center:
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

        [Display(Name = "CSS Files", GroupName = Styles, Order = 100)]
        public virtual LinkItemCollection CssFiles { get; set; }

        [Display(GroupName = Styles, Order = 110)]
        [UIHint(UIHint.Textarea)]
        public virtual string Css { get; set; }

        #endregion

        #region Scripts

        [Display(
            Name = "Script Files",
            GroupName = ScriptTag,
            Order = 100)]
        public virtual LinkItemCollection ScriptFiles { get; set; }

        [Display(GroupName = ScriptTag, Order = 110)]
        [UIHint(UIHint.Textarea)]
        public virtual string Scripts { get; set; }

        #endregion

        #region Page Settings

        [Display(GroupName = CmsTabs.PageSettings,
            Order = 200)]
        [CultureSpecific]
        public virtual bool HideSiteHeader { get; set; }

        [Display(
            GroupName = CmsTabs.PageSettings,
            Order = 300)]
        [CultureSpecific]
        public virtual bool HideSiteFooter { get; set; }

        [Display(
            Name = "Page Main Image",
            Description = "",
            GroupName = CmsTabs.PageSettings,
            Order = 6)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference PageImage { get; set; }

        [CultureSpecific]
        [Display(Name = "Display hover effect", GroupName = CmsTabs.PageSettings, Order = 100)]
        public virtual bool ApplyHoverEffect { get; set; }

        [Display(
            Name = "Exclude From Search",
            Description = "This will determine whether or not to show on search.",
            GroupName = CmsTabs.PageSettings,
            Order = 600)]
        [CultureSpecific]
        public virtual bool ExcludeFromSearch { get; set; }

        #endregion
    }
}