using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.SiteSettings.Interfaces;

namespace Foundation.Cms.SiteSettings.Pages
{
    [ContentType(DisplayName = "Selection Settings Page", 
        GUID = "4356a392-ed29-4895-9e65-bf44fa3db5ca", 
        Description = "")]
    public class SelectionSettingsPage : SettingsBasePage, ISelectionSettings
    {
        #region Person settings
        [Display(GroupName = CmsTabNames.CustomSettings, Order = 100)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<SelectionItem>))]
        public virtual IList<SelectionItem> Sectors { get; set; }

        [Display(GroupName = CmsTabNames.CustomSettings, Order = 200)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<SelectionItem>))]
        public virtual IList<SelectionItem> Locations { get; set; }
        #endregion

        #region Color settings
        [Display(Name = "Color options", GroupName = CmsTabNames.Styles, Order = 130)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ColorModel>))]
        public virtual IList<ColorModel> ColorOptions { get; set; }
        #endregion
    }

    public class ColorModel
    {
        public string Property { get; set; }
        public string Value { get; set; }
        [Display(Name = "Color name")]
        public string ColorName { get; set; }

        [Display(Name = "Color code")]
        [ClientEditor(ClientEditingClass = "dijit/ColorPalette")]
        public string ColorCode { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class ColorPropertyList : PropertyList<ColorModel>
    {
    }
}