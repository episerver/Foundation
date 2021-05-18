using EPiServer.DataAnnotations;
using EPiServer.Globalization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Foundation.Infrastructure.Find.Facets.Config
{
    public class FacetFilterConfigurationItem
    {
        public FacetFilterConfigurationItem()
        {
            FieldType = FacetFieldType.String.ToString();
            DisplayMode = FacetDisplayMode.Checkbox.ToString();
            DisplayDirection = FacetDisplayDirection.Vertical.ToString();
        }

        [Display(
           Name = "Attribute as Filter (required)",
           Description = "Attribute to be used as a filter",
           Order = 1)]
        [Required]
        public virtual string FieldName { get; set; }

        [Display(
           Name = "Display Name",
           Description = "Display name for filter in English")]
        public virtual string DisplayNameEN { get; set; }

        [Display(
          Name = "Display Name (FR)",
          Description = "Display name for filter in French")]
        [Ignore]
        [ScaffoldColumn(false)]
        public virtual string DisplayNameFR { get; set; }

        public string GetDisplayName()
        {
            return string.Equals(ContentLanguage.PreferredCulture.Name, "en", StringComparison.InvariantCultureIgnoreCase)
                    ? DisplayNameEN
                    : DisplayNameFR;
        }

        [Display(
            Name = "Filter Type (required)",
            Description = "Data type of attribute")]
        [SelectOneEnum(typeof(FacetFieldType))]
        [DefaultValue(FacetFieldType.String)]
        [Required]
        public virtual string FieldType { get; set; }

        public Type GetFieldType()
        {
            if (!string.IsNullOrEmpty(FieldType) && Enum.TryParse(FieldType, out FacetFieldType facetFieldType))
            {
                switch (facetFieldType)
                {
                    case FacetFieldType.ListOfString:
                        return typeof(IList<string>);
                    case FacetFieldType.Integer:
                        return typeof(int);
                    case FacetFieldType.Double:
                        return typeof(double);
                    case FacetFieldType.Boolean:
                        return typeof(bool);
                    case FacetFieldType.NullableBoolean:
                        return typeof(bool?);
                    default:
                        return typeof(string);
                }
            }

            return typeof(string);
        }

        [Display(
            Name = "Display Mode (required)",
            Description = "How the values of the filter are displayed")]
        [SelectOneEnum(typeof(FacetDisplayMode))]
        [DefaultValue(FacetDisplayMode.Button)]
        [Required]
        public virtual string DisplayMode { get; set; }

        public FacetDisplayMode GetDisplayMode()
        {
            if (!string.IsNullOrEmpty(DisplayMode) && Enum.TryParse(DisplayMode, out FacetDisplayMode facetDisplayMode))
            {
                return facetDisplayMode;
            }

            return FacetDisplayMode.Checkbox;
        }

        [Display(
              Name = "Display direction (optional)",
              Description = "Only applies to color swatch and size swatch.")]
        [SelectOneEnum(typeof(FacetDisplayDirection))]
        [DefaultValue(FacetDisplayDirection.Vertical)]
        public virtual string DisplayDirection { get; set; }

        public FacetDisplayDirection GetDisplayDirection()
        {
            if (!string.IsNullOrEmpty(DisplayDirection) && Enum.TryParse(DisplayDirection, out FacetDisplayDirection facetDisplayDirection))
            {
                return facetDisplayDirection;
            }

            return FacetDisplayDirection.Vertical;
        }

        [Display(
            Name = "Numeric Ranges (From-To)",
            Description = "Set ranges based on field type in format: from-to, from- and -to. E.g. range 1:0-10/range 2:11-20; range 1: 1.00-5.50/ range 2:5.51-10.25; 20.12-/-500.24")]
        [ItemRegularExpression("[0-9]*\\.?[0-9]*-[0-9]*\\.?[0-9]*")]
        public virtual IList<string> NumericRanges { get; set; }

        public List<SelectableNumericRange> GetSelectableNumericRanges()
        {
            if (NumericRanges != null && NumericRanges.Any())
            {
                var numericValues = NumericRanges.Select(value =>
                {
                    var arr = value.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    switch (arr.Length)
                    {
                        case 2:
                            return new SelectableNumericRange() { From = Convert.ToDouble(arr[0]), To = Convert.ToDouble(arr[1]) };
                        case 1:
                            if (value.StartsWith("-"))
                            {
                                return new SelectableNumericRange() { To = Convert.ToDouble(arr[0]) };
                            }
                            else
                            {
                                return new SelectableNumericRange() { From = Convert.ToDouble(arr[0]) };
                            }
                        default:
                            return new SelectableNumericRange();
                    }

                })
                .ToList();

                return numericValues;
            }

            return new List<SelectableNumericRange>();
        }

        [Display(
            Name = "Exclude Flag Attributes or Specific Values",
            Description = "Used to exclude specific attributes from Flags or specific values of an attribute")]
        public virtual IList<string> ExcludeFlagFields { get; set; }

        [Display(
            Name = "Display Specific Values",
            Description = "Used to display specific values of an Attribute as Filter: e.g. Brand. Must be exact match to value of attribute.")]
        public virtual IList<string> DisplaySpecificValues { get; set; }
    }
}
