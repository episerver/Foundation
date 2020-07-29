using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Shared.SelectionFactories
{
    public class BackgroundColorSelectionFactory : ISelectionFactory
    {
        private readonly LocalizationService _localizationService;

        public BackgroundColorSelectionFactory() : this(ServiceLocator.Current.GetInstance<LocalizationService>())
        {
        }

        public BackgroundColorSelectionFactory(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }


        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text =  _localizationService.GetString("None", "None"), Value = "transparent" },
                new SelectItem { Text =  _localizationService.GetString("Black", "Black"), Value = "black" },
                new SelectItem { Text =  _localizationService.GetString("Grey", "Grey"), Value = "grey" },
                new SelectItem { Text =  _localizationService.GetString("Beige", "Beige"), Value = "beige" },
                new SelectItem { Text =  _localizationService.GetString("Light Blue", "Light Blue"), Value = "#0081b2" },
                new SelectItem { Text =  _localizationService.GetString("Yellow", "Yellow"), Value = "#fec84d" }
            };
        }
    }
}
