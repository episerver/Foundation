using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.Pages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Comparison Page",
        GUID = "73fb146b-ff08-4e97-8b21-dedafa6a121e",
        Description = "The page used to compare 2 variants.",
        AvailableInEditMode = false,
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-29.png")]
    public class ComparisonPage : FoundationPageData
    {
        [Display(Name = "Comparison properties", GroupName = SystemTabNames.Content, Order = 100)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ComparisonProperty>))]
        public virtual IList<ComparisonProperty> ComparisonProperties { get; set; }
    }

    public class ComparisonProperty
    {
        [Display(Name = "Display name", Order = 10)]
        public string DisplayName { get; set; }

        [Display(Name = "Property name", Order = 20)]
        public string PropertyName { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class ComparisonPropertyPropertyList : PropertyList<ComparisonProperty> { }
}