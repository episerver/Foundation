using FileHelpers;

namespace Foundation.Features.MyOrganization.QuickOrderPage
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines]
    public class QuickOrderData
    {
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public string Sku;

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public int Quantity;
    }
}