using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    public class NumericOperatorSelectionFactory : ISelectionFactory
    {
        public static class OperatorNames
        {
            public const string Equal = "Equal";
            public const string GreaterThan = "GreaterThan";
            public const string LessThan = "LessThan";
        }

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
             {
                new SelectItem { Text = "Equals", Value = OperatorNames.Equal },
                new SelectItem { Text = "Greater Than", Value = OperatorNames.GreaterThan },
                new SelectItem { Text = "Less Than", Value = OperatorNames.LessThan },
             };
        }
    }
}