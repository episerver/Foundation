using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Commerce;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using Foundation.Features.CatalogContent.Variation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicVariation
{
    [CatalogContentType(DisplayName = "Dynamic Variant", GUID = "11c2960f-79d6-4876-8d8a-6b7bc8cfe869", Description = "Dynamic variant supports multiple variation types")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-23.png")]
    public class DynamicVariant : GenericVariant
    {
        [BackingType(typeof(VariantGroupPropertyList))]
        [Display(GroupName = "Variants Options", Order = 400)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<VariantGroup>))]
        public virtual IList<VariantGroup> VariantsOptions { get; set; }
    }


    public class VariantGroup
    {
        [UIHint(UIHint.ProductVariation)]
        [Display(Name = "Variation")]
        public virtual ContentReference Variant { get; set; }

        [Display(Name = "Group name")]
        public virtual string GroupName { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class VariantGroupPropertyList : PropertyList<VariantGroup>
    {
        protected override VariantGroup ParseItem(string value)
        {
            return JsonConvert.DeserializeObject<VariantGroup>(value);
        }
        
        [Obsolete("ParseToObject is no longer required to be implemented. The same functionality can be achieved by creating a new instance and calling the ParseToSelf method.")]
        public override PropertyData ParseToObject(string value)
        {
            var prop = new VariantGroupPropertyList();
            prop.ParseToSelf(value);
            return prop;
        }

        public override string ToString()
        {
            if (IsNull)
            {
                return string.Empty;
            }
            return string.Join(StringRepresentationSeparator.ToString(),
                List.Select(x => x.ToString()));
            //this needs to handle representing the object as a string
        }
    }
}