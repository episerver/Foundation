namespace Foundation.Infrastructure.Commerce.Models.EditorDescriptors
{
    public class CurrencySelector : ISelectionFactory
    {
        //public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        //{
        //    var items = new List<SelectItem>();
        //    items.Insert(0, new SelectItem { Text = "All", Value = "All" });
        //    items.Add(new SelectItem() { Text = "AUD", Value = "AUD" });
        //    items.Add(new SelectItem() { Text = "BRL", Value = "BRL" });
        //    items.Add(new SelectItem() { Text = "CAD", Value = "CAD" });
        //    items.Add(new SelectItem() { Text = "EUR", Value = "EUR" });
        //    items.Add(new SelectItem() { Text = "USD", Value = "USD" });
        //    return items;
        //}

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var items = new List<SelectItem>();
            items.Insert(0, new SelectItem { Text = "All", Value = "All" });
            items.Add(new SelectItem() { Text = "AUD", Value = "AUD" });
            items.Add(new SelectItem() { Text = "BRL", Value = "BRL" });
            items.Add(new SelectItem() { Text = "CAD", Value = "CAD" });
            items.Add(new SelectItem() { Text = "EUR", Value = "EUR" });
            items.Add(new SelectItem() { Text = "USD", Value = "USD" });
            return items;
        }
    }
}