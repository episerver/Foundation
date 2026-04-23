using EPiServer.Commerce.Catalog.ContentTypes;
using System.Reflection;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    [ContentType(DisplayName = "Numeric Filter Block",
        GUID = "7747D13C-D029-4CB5-B020-549676123AC4",
        Description = "Filter product search blocks by numeric field values",
        GroupName = "Commerce")]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-14.png")]
    public class NumericFilterBlock : FilterBaseBlock
    {
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(NumericOperatorSelectionFactory))]
        [Display(Name = "Operator", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string FieldOperator { get; set; }

        [CultureSpecific]
        [Display(Name = "Value", Description = "The value to filter search results on", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual double FieldValue { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            FieldOperator = NumericOperatorSelectionFactory.OperatorNames.Equal;
        }

        public override Func<EntryContentBase, bool> GetPredicate()
        {
            if (string.IsNullOrEmpty(FieldName))
                return null;

            return entry =>
            {
                var prop = entry.GetType().GetProperty(FieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) return true; // property not found — don't exclude
                double val;
                try { val = Convert.ToDouble(prop.GetValue(entry) ?? 0); }
                catch { return true; }

                return FieldOperator switch
                {
                    NumericOperatorSelectionFactory.OperatorNames.GreaterThan => val > FieldValue,
                    NumericOperatorSelectionFactory.OperatorNames.LessThan => val < FieldValue,
                    _ => Math.Abs(val - FieldValue) < 0.001
                };
            };
        }
    }
}
