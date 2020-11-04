using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Commerce.Catalog.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Validation;
using EPiServer.Web;
using Foundation.Features.CatalogContent.Variation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicVariation
{
    [CatalogContentType(DisplayName = "Dynamic Variant", GUID = "11c2960f-79d6-4876-8d8a-6b7bc8cfe869", Description = "Dynamic variant supports multiple options")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-23.png")]
    public class DynamicVariant : GenericVariant
    {
        [BackingType(typeof(VariantGroupPropertyList))]
        [Display(Name = "Variant Options", GroupName = "Variant Options", Order = 400)]
        [ClientEditor(ClientEditingClass = "foundation/VariantOptionPrices")]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<VariantGroup>))]
        public virtual IList<VariantGroup> VariantOptions { get; set; }
    }

    public class PageListBlockValidator : IValidate<DynamicVariant>
    {
        public IEnumerable<ValidationError> Validate(DynamicVariant variant)
        {
            if (variant.VariantOptions != null && variant.VariantOptions.Any())
            {
                if (variant.VariantOptions.GroupBy(x => x.Code).Where(x => x.Count() > 1).Any())
                {
                    return new ValidationError[]
                    {
                        new ValidationError()
                        {
                             ErrorMessage = "The variant option code is unique",
                             PropertyName = variant.GetPropertyName(x => x.VariantOptions),
                             Severity = ValidationErrorSeverity.Error,
                             ValidationType = ValidationErrorType.StorageValidation
                        }
                    };
                }
            }

            return Enumerable.Empty<ValidationError>();
        }
    }

    public class VariantGroup
    {
        [Display(Name = "Group name")]
        public virtual string GroupName { get; set; }

        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Code { get; set; }

        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [Required]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<PriceModel>))]
        public virtual IList<PriceModel> Prices { get; set; }
    }

    public class PriceModel
    {
        public decimal Amount { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CurrencySelectionFactory))]
        public string Currency { get; set; }
    }


    [PropertyDefinitionTypePlugIn]
    public class PriceModelPropertyList : PropertyList<PriceModel>
    {
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