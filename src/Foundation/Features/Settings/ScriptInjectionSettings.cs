using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using Foundation.Cms.Settings;
using Foundation.Features.Folder;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Scripts Injection Settings",
        GUID = "0156b963-88a9-450b-867c-e5c5e7be29fd",
        Description = "Scripts Injection Settings",
        SettingsName = "Scripts Injection")]
    public class ScriptInjectionSettings : SettingsBase
    {
        #region Scripts

        [JsonIgnore]
        [CultureSpecific]
        [Display(Name = "Header Scripts (Scripts will inject at the bottom of header)", GroupName = TabNames.Scripts, Description = "Scripts will inject at the bottom of header", Order = 10)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ScriptInjectionModel>))]
        [ClientEditor(ClientEditingClass = "foundation/ScriptInjectionItems")]
        public virtual IList<ScriptInjectionModel> HeaderScripts { get; set; }

        [JsonIgnore]
        [CultureSpecific]
        [Display(Name = "Header Scripts (Scripts will inject at the bottom of footer)", GroupName = TabNames.Scripts, Description = "Scripts will inject at the bottom of footer", Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ScriptInjectionModel>))]
        [ClientEditor(ClientEditingClass = "foundation/ScriptInjectionItems")]
        public virtual IList<ScriptInjectionModel> FooterScripts { get; set; }

        #endregion
    }

    public class ScriptInjectionModel
    {
        [CultureSpecific]
        [AllowedTypes(typeof(FoundationPageData), typeof(FolderPage))]
        [Display(Name = "Root (Scripts will inject for this page and all children pages)", Description = "Scripts will inject for this page and all children pages", Order = 10)]
        public virtual ContentReference ScriptRoot { get; set; }

        [Display(Name = "Script files", Order = 20)]
        public virtual LinkItemCollection ScriptFiles { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class ScriptInjectionProperty : PropertyList<ScriptInjectionModel>
    {
    }
}