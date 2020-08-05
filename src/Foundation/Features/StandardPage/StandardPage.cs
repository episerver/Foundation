using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Features.Shared.SelectionFactories;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.StandardPage
{
    [ContentType(DisplayName = "Standard Page",
        GUID = "c0a25bb7-199c-457d-98c6-b0179c7acae8",
        Description = "Allows for creation of rich standard pages",
        GroupName = SystemTabNames.Content)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-23.png")]
    public class StandardPage : FoundationPageData
    {
        [CultureSpecific]
        [ClientEditor(ClientEditingClass = "dijit/ColorPalette")]
        [Display(Name = "Title color", GroupName = SystemTabNames.Content, Order = 203)]
        public virtual string TitleColor
        {
            get => this.GetPropertyValue(page => page.TitleColor) ?? "white";
            set => this.SetPropertyValue(page => page.TitleColor, value);
        }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Background image", GroupName = SystemTabNames.Content, Order = 206)]
        public virtual ContentReference BackgroundImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
        [Display(Name = "Background video", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentReference BackgroundVideo { get; set; }

        [SelectOne(SelectionFactoryType = typeof(TopPaddingModeSelectionFactory))]
        [Display(Name = "Top padding mode",
            Description = "Sets how much padding should be at the top of the standard content",
            GroupName = SystemTabNames.Content,
            Order = 220)]
        public virtual string TopPaddingMode { get; set; }

        [SelectOne(SelectionFactoryType = typeof(BackgroundColorSelectionFactory))]
        [Display(Name = "Background color", GroupName = SystemTabNames.Content, Order = 204)]
        public virtual string BackgroundColor
        {
            get => this.GetPropertyValue(page => page.BackgroundColor) ?? "transparent";
            set => this.SetPropertyValue(page => page.BackgroundColor, value);
        }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Name = "Background opacity (0 to 1)", GroupName = SystemTabNames.Content, Order = 205)]
        public virtual double? BackgroundOpacity
        {
            get => this.GetPropertyValue(page => page.BackgroundOpacity) ?? 1;
            set => this.SetPropertyValue(page => page.BackgroundOpacity, value);
        }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            BackgroundColor = "transparent";
            BackgroundOpacity = 1;
            TitleColor = "white";
            TopPaddingMode = TopPaddingModeSelectionFactory.TopPaddingModes.Half;
        }
    }
}