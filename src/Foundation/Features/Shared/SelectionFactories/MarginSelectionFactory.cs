using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Shared.SelectionFactories
{
    public class MarginSelectionFactory : ISelectionFactory
    {
        private readonly LocalizationService _localizationService;

        public MarginSelectionFactory() : this(ServiceLocator.Current.GetInstance<LocalizationService>())
        {
        }

        public MarginSelectionFactory(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = _localizationService.GetString("None", "None"), Value = "m-0" },
                new SelectItem { Text = _localizationService.GetString("Small", "Small"), Value = "m-1" },
                new SelectItem { Text = _localizationService.GetString("Medium", "Medium"), Value = "m-3" },
                new SelectItem { Text = _localizationService.GetString("Large", "Large"), Value = "m-5" },
            };
        }
    }
}
