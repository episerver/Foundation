namespace Foundation.Features.Blocks.ModalBlock
{
    public class ModalSizeSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Default (500px)", Value = "" },
                new SelectItem { Text = "Small (300px)", Value = "modal-sm" },
                new SelectItem { Text = "Large (800px)", Value = "modal-lg" },
                new SelectItem { Text = "Extra Large (1140px)", Value = "modal-xl" },
            };
        }
    }
}
