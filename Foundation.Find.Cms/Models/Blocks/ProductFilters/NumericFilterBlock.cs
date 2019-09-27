using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Framework;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms.Models.Blocks.ProductFilters
{
    [ContentType(DisplayName = "Numeric Filter",
        GUID = "7747D13C-D029-4CB5-B020-549676123AC4",
        Description = "Filter product search blocks by field values",
        GroupName = "Commerce")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-14.png")]
    public class NumericFilterBlock : FilterBaseBlock
    {

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 25,
            Name = "Filter Value",
            Description = "The value to filter search results on")]
        [CultureSpecific(true)]
        public virtual double FieldValue { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 20,
            Name = "Operator")]
        [CultureSpecific(true)]
        [SelectOne(SelectionFactoryType = typeof(NumericOperatorSelectionFactory))]
        public virtual string FieldOperator { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            FieldOperator = NumericOperatorSelectionFactory.OperatorNames.Equal;
        }

        public override Filter GetFilter()
        {
            if (string.IsNullOrEmpty(FieldName))
            {
                return null;
            }

            var fullFieldName = SearchClient.Instance.GetFullFieldName(FieldName, typeof(double));
            switch (FieldOperator)
            {
                case NumericOperatorSelectionFactory.OperatorNames.GreaterThan:
                    var greaterThanFilter = RangeFilter.Create(fullFieldName, FieldValue, double.MaxValue);
                    greaterThanFilter.IncludeLower = false;
                    greaterThanFilter.IncludeUpper = true;
                    return greaterThanFilter;
                case NumericOperatorSelectionFactory.OperatorNames.LessThan:
                    var lessThanFilter = RangeFilter.Create(fullFieldName, double.MinValue, FieldValue);
                    lessThanFilter.IncludeLower = false;
                    lessThanFilter.IncludeUpper = true;
                    return lessThanFilter;
                default:
                    return new TermFilter(fullFieldName, FieldValue);

            }
        }
    }
}