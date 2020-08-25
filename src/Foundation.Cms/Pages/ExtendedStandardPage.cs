using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms.Blocks;
using Foundation.Cms.EditorDescriptors;

namespace Foundation.Cms.Pages
{
    [ContentType(DisplayName = "Extended Standard Page",
        GUID = "e6911472-0769-4092-a53c-7a6241df4dc3",
        Description = "Allows for creation of rich extended standard pages",
        GroupName = SystemTabNames.Content)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-23.png")]
    public class ExtendedStandardPage : FoundationPageData
    {
        [CultureSpecific]
        [Editable(false)]
        public override ContentArea MainContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Left content area", GroupName = SystemTabNames.Content, Order = 201)]
        public virtual ContentArea LeftContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Right content area", GroupName = SystemTabNames.Content, Order = 202)]
        public virtual ContentArea RightContentArea { get; set; }

        [CultureSpecific]
        [ClientEditor(ClientEditingClass = "dijit/ColorPalette")]
        [Display(Name = "Title color", GroupName = SystemTabNames.Content, Order = 203)]
        public virtual string TitleColor
        {
            get { return this.GetPropertyValue(page => page.TitleColor) ?? "white"; }
            set { this.SetPropertyValue(page => page.TitleColor, value); }
        }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Background image", GroupName = SystemTabNames.Content, Order = 206)]
        public virtual ContentReference BackgroundImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
        [Display(Name = "Background video", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentReference BackgroundVideo { get; set; }

        [SelectOne(SelectionFactoryType = typeof(FoundationStandardPageTopPaddingModeSelectionFactory))]
        [Display(Name = "Top padding mode",
            Description = "Sets how much padding should be at the top of the standard content",
            GroupName = SystemTabNames.Content,
            Order = 220)]
        public virtual string TopPaddingMode { get; set; }

        [SelectOne(SelectionFactoryType = typeof(BackgroundColorSelectionFactory))]
        [Display(Name = "Background color", GroupName = SystemTabNames.Content, Order = 204)]
        public virtual string BackgroundColor
        {
            get { return this.GetPropertyValue(page => page.BackgroundColor) ?? "transparent"; }
            set { this.SetPropertyValue(page => page.BackgroundColor, value); }
        }

        [Range(0, 1.0, ErrorMessage = "Opacity only allows value between 0 and 1")]
        [Display(Name = "Background opacity (0 to 1)", GroupName = SystemTabNames.Content, Order = 205)]
        public virtual double? BackgroundOpacity
        {
            get { return this.GetPropertyValue(page => page.BackgroundOpacity) ?? 1; }
            set { this.SetPropertyValue(page => page.BackgroundOpacity, value); }
        }

        [AllowedTypes(new[] {typeof(AdsBlock)})]
        [Display(Name = "Header", GroupName = SystemTabNames.Content, Order = 211)]
        public virtual ContentArea HeaderAds { get; set; }

        [AllowedTypes(new[] { typeof(AdsBlock) })]
        [Display(Name = "Footer", GroupName = SystemTabNames.Content, Order = 212)]
        public virtual ContentArea FooterAds { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            BackgroundColor = "transparent";
            BackgroundOpacity = 1;
            TitleColor = "white";
            TopPaddingMode = FoundationStandardPageTopPaddingModeSelectionFactory.FoundationStandardPageTopPaddingModes.Half;
        }
    }
}
