using FileHelpers;

namespace Foundation.Commerce.Customer.ViewModels
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