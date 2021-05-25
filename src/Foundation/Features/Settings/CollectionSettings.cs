﻿using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms;
using Foundation.Cms.Settings;
using Foundation.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Collection Settings",
        GUID = "4356a392-ed29-4895-9e65-bf44fa3db5ca",
        Description = "Selection options settings",
        SettingsName = "Collection Settings")]
    public class CollectionSettings : SettingsBase
    {
        #region Person settings

        [Display(GroupName = TabNames.CustomSettings, Order = 100)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<SelectionItem>))]
        public virtual IList<SelectionItem> Sectors { get; set; }

        [Display(GroupName = TabNames.CustomSettings, Order = 200)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<SelectionItem>))]
        public virtual IList<SelectionItem> Locations { get; set; }

        #endregion

        #region Color settings

        [Display(Name = "Background colors", GroupName = TabNames.Colors, Order = 10)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ColorModel>))]
        public virtual IList<ColorModel> BackgroundColor { get; set; }

        [Display(Name = "Heading colors", GroupName = TabNames.Colors, Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ColorModel>))]
        public virtual IList<ColorModel> HeadingColor { get; set; }

        [Display(Name = "Text colors", GroupName = TabNames.Colors, Order = 30)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ColorModel>))]
        public virtual IList<ColorModel> TextColor { get; set; }

        [Display(Name = "Block opacity background colors", GroupName = TabNames.Colors, Order = 40)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ColorModel>))]
        public virtual IList<ColorModel> OpacityBackgrounColor { get; set; }

        [Display(Name = "Button background colors", GroupName = TabNames.Colors, Order = 50)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ColorModel>))]
        public virtual IList<ColorModel> ButtonBackgrounColor { get; set; }

        [Display(Name = "Button text colors", GroupName = TabNames.Colors, Order = 60)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ColorModel>))]
        public virtual IList<ColorModel> ButtonTextColor { get; set; }

        [Display(Name = "Banner background color", GroupName = TabNames.Colors, Order = 60)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string BannerBackgroundColor { get; set; }

        [Display(Name = "Banner text color", GroupName = TabNames.Colors, Order = 70)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string BannerTextColor { get; set; }

        [Display(Name = "Link color", GroupName = TabNames.Colors, Order = 80)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string LinkColor { get; set; }

        #endregion
    }

    public class ColorModel
    {
        [Display(Name = "Color name")]
        public string ColorName { get; set; }

        [Display(Name = "Color code")]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public string ColorCode { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class ColorPropertyList : PropertyList<ColorModel>
    {
    }

    [PropertyDefinitionTypePlugIn]
    public class SelectionItemProperty : PropertyList<SelectionItem>
    {
    }
}