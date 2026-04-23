using EPiServer.Commerce.Catalog.ContentTypes;

namespace Foundation.Features.Blocks.ProductFilterBlocks
{
    public abstract class FilterBaseBlock : BlockData
    {
        [CultureSpecific]
        [Display(Name = "Field name", Description = "Name of the product property to filter on", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string FieldName { get; set; }

        /// <summary>
        /// Returns an in-memory predicate to apply to catalog entries, or null if not fully configured.
        /// Replaces the EPiServer.Find Filter objects removed in CMS 13.
        /// </summary>
        public abstract Func<EntryContentBase, bool> GetPredicate();
    }
}
