using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Infrastructure;
using Foundation.Infrastructure.Cms.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Font Settings",
       GUID = "b0fd87d2-b7ed-4711-8a1a-5127fdae24f1",
       SettingsName = "Site Fonts")]

    public class FontSettings : SettingsBase
    {

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(GlobalFontSelectionFactory))]
        [Display(Name = "Global Font", GroupName = "Global", Order = 0)]
        public virtual string GlobalFontDropDown { get; set; }

        [Display(Name = "Google Fonts", GroupName = "Google Fonts", Order = 10)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<FontModel>))]
        public virtual IList<FontModel> FontFields { get; set; }

        [PropertyDefinitionTypePlugIn]
        public class FieldsProperty : PropertyList<FontModel> { }

        [Display(Name = "Custom Fonts", GroupName = "Custom Fonts", Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<CustomFontModel>))]
        public virtual IList<CustomFontModel> CustomFonts { get; set; }

        [PropertyDefinitionTypePlugIn]
        public class CustomFontFieldsProperty : PropertyList<CustomFontModel> { }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            GlobalFontDropDown = "Initial";
        }
    }

    public class GlobalFontSelectionFactory : ISelectionFactory
    {
        private static readonly Lazy<ISettingsService> _settingsService =
          new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            SortedDictionary<string, string> fonts = new SortedDictionary<string, string>();

            

            var settings = _settingsService.Value.GetSiteSettings<FontSettings>();

            if (settings != null && settings.FontFields != null)
            {
                for (var i = 0; i < settings.FontFields.Count; i++)
                {
                    if (settings.FontFields[i].EnableFont)
                    {
                        fonts.Add(settings.FontFields[i].FontName, settings.FontFields[i].FontName);
                        
                    }
                }
            }

            if (settings != null && settings.CustomFonts != null)
            {
                for (var i = 0; i < settings.CustomFonts.Count; i++)
                {
                    if (settings.CustomFonts[i].EnableFont)
                    {
                        fonts.Add(settings.CustomFonts[i].FontName, settings.CustomFonts[i].FontName);

                    }
                }
            }

            fonts.Add("- Default Site Font -", "Initial");

            return fonts.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
       
        }
    }

    public class FontModel
    {
        [Display(Name = "Enable Font")]
        public bool EnableFont { get; set; }

        [Display(Name = "Font Name")]
        public string FontName { get; set; }

        [Display(Name = "Font Import")]
        public string FontImport { get; set; }

    }

    public class CustomFontModel
    {
        [Display(Name = "Enable Font")]
        public bool EnableFont { get; set; }

        [Display(Name = "Font Name")]
        public string FontName { get; set; }

        [Display(Name = "Font File")]
        public LinkItemCollection FontFile { get; set; }

        internal IEnumerable<object> Where(Func<object, bool> value) => throw new NotImplementedException();
    }

    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = UiHint)]
    public class TallWideTextboxEditorDescriptor : EditorDescriptor
    {
        public const string UiHint = "TallWideTextbox";

        public TallWideTextboxEditorDescriptor()
        {
            ClientEditingClass = "epi/shell/widget/ValidationTextarea";
            Style = "width:600px; height:400px;";
        }

        protected override void SetEditorConfiguration(ExtendedMetadata metadata)
        {
            EditorConfiguration["style"] = Style;
            base.SetEditorConfiguration(metadata);
        }

        // Properties
        public string Style { get; set; }
    }


}
