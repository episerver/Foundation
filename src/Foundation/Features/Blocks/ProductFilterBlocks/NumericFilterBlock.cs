using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Framework;
using EPiServer.Shell.ObjectEditing;
using Foundation.Find;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    [ContentType(DisplayName = "Numeric Filter Block",
        GUID = "7747D13C-D029-4CB5-B020-549676123AC4",
        Description = "Filter product search blocks by field values",
        GroupName = "Commerce")]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-14.png")]
    public class NumericFilterBlock : FilterBaseBlock
    {
        [CultureSpecific(true)]
        [SelectOne(SelectionFactoryType = typeof(NumericOperatorSelectionFactory))]
        [Display(Name = "Operator", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string FieldOperator { get; set; }

        [CultureSpecific(true)]
        [Display(Name = "Value", Description = "The value to filter search results on", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual double FieldValue { get; set; }

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