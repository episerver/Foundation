using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Shared.SelectionFactories
{
    public class PaddingSelectionFactory : ISelectionFactory
    {
        private readonly LocalizationService _localizationService;

        public PaddingSelectionFactory() : this(ServiceLocator.Current.GetInstance<LocalizationService>())
        {
        }

        public PaddingSelectionFactory(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = _localizationService.GetString("None", "None"), Value = "p-0" },
                new SelectItem { Text = _localizationService.GetString("Small", "Small"), Value = "p-1" },
                new SelectItem { Text = _localizationService.GetString("Medium", "Medium"), Value = "p-3" },
                new SelectItem { Text = _localizationService.GetString("Large", "Large"), Value = "p-5" },
            };
        }
    }
}
