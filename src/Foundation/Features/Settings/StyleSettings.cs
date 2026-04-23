using Microsoft.AspNetCore.Mvc;
using Foundation.Infrastructure.Cms.Settings;


namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Style Settings",
       GUID = "de429deddc40403d9d06cf71d5fab8d7",
       SettingsName = "Style Settings")]

    public class StyleSettings : SettingsBase
    {

        #region Buttons

        [CultureSpecific]
        [Display(Name = "Override Global Button Styles", GroupName = "Buttons", Description = "Check off to override the site buttons with the settings here", Order = 0)]
        public virtual bool OverrideGlobalButtonStyles { get; set; }

        [CultureSpecific]
        [Display(Name = "Uppercase", GroupName = "Buttons", Order = 10)]
        public virtual bool ButtonUppercase { get; set; }

        [Display(Name = "Font Size", GroupName = "Buttons", Order = 20)]
        [SelectOne(SelectionFactoryType = typeof(ButtonFontSizeFactory))]
        public virtual string ButtonTextFontSize { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Text Padding", GroupName = "Buttons", Order = 30)]
        public virtual string ButtonTextPadding { get; set; }

        [Display(Name = "Border width (px)", GroupName = "Buttons", Order = 40)]
        [RegularExpression("^[+]?\\d*$", ErrorMessage = "Border Width must be non-negative")]
        public virtual int ButtonBorderWidth { get; set; }

        [SelectOne(SelectionFactoryType = typeof(PaddingSelectionFactory))]
        [Display(Name = "Border Radius", GroupName = "Buttons", Order = 50)]
        public virtual string ButtonBorderRadius { get; set; }

        [CultureSpecific]
        [ClientEditor(ClientEditingClass = "foundation/Editors/ColorPicker")]
        [Display(Name = "Button Text color", GroupName = "Buttons", Order = 60)]
        public virtual string ButtonTextColor { get; set; }

        [CultureSpecific]
        [ClientEditor(ClientEditingClass = "foundation/Editors/ColorPicker")]
        [Display(Name = "Button Text Hover color", GroupName = "Buttons", Order = 70)]
        public virtual string ButtonTextHoverColor { get; set; }
        
        [ClientEditor(ClientEditingClass = "foundation/Editors/ColorPicker")]
        [Display(Name = "Button Background color", GroupName = "Buttons", Order = 80)]
        public virtual string ButtonBackgroundColor { get; set; }

        [ClientEditor(ClientEditingClass = "foundation/Editors/ColorPicker")]
        [Display(Name = "Button Hover Background color", GroupName = "Buttons", Order = 90)]
        public virtual string ButtonHoverBackgroundColor { get; set; }

        [CultureSpecific]
        [ClientEditor(ClientEditingClass = "foundation/Editors/ColorPicker")]
        [Display(Name = "Button Border color", GroupName = "Buttons", Order = 100)]
        public virtual string ButtonBorderColor { get; set; }

        [CultureSpecific]
        [ClientEditor(ClientEditingClass = "foundation/Editors/ColorPicker")]
        [Display(Name = "Button Border Hover color", GroupName = "Buttons", Order = 110)]
        public virtual string ButtonBorderHoverColor { get; set; }

        #endregion
        #region Cards

        [CultureSpecific]
        [Display(Name = "Override Card Styles", GroupName = "Cards", Description = "Check off to override the card settings", Order = 0)]
        public virtual bool OverrideCardStyles { get; set; }

        [Display(Name = "Card Image Height", GroupName = "Cards", Order = 10)]
        public virtual string CardImageHeight { get; set; }

        [Display(Name = "Card Min Height", GroupName = "Cards", Order = 20)]
        public virtual string CardMinHeight { get; set; }

        #endregion
        #region Forms

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(TopBannerAlignSelectionFactory))]
        [Display(Name = "Form Alignments", GroupName = "Forms", Order = 10)]
        public virtual string FormAlign { get; set; }

        #endregion


        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ButtonUppercase = true;
            ButtonTextFontSize = "medium";
            ButtonTextPadding = "p-3";
            ButtonTextColor = "#000000ff";
            ButtonTextHoverColor = "#ffffff00";
            ButtonBackgroundColor = "#ffffff00";
            ButtonHoverBackgroundColor = "#ffffff00";
            ButtonBorderWidth = 1;
            ButtonBorderColor = "#000000ff";
            ButtonBorderHoverColor = "#000000ff";
            ButtonBorderRadius = "p-0";
            FormAlign = "left";

        }

        public class ButtonFontSizeFactory : ISelectionFactory
        {
            public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
            {
                return new ISelectItem[]
                {
                    new SelectItem { Text = "Initial", Value = "initial" },
                    new SelectItem { Text = "XX-Small", Value = "xx-small" },
                    new SelectItem { Text = "X-Small", Value = "smaller" },
                    new SelectItem { Text = "Small", Value = "small" },
                    new SelectItem { Text = "Medium	", Value = "medium" },
                    new SelectItem { Text = "Large", Value = "large" },
                    new SelectItem { Text = "X-Large", Value = "x-large" },
                    new SelectItem { Text = "XX-Large", Value = "xx-large" },
                    new SelectItem { Text = "XXX-Large", Value = "xxx-large" }
                    
                };
            }
        }

    }
    
}
