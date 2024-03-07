using Foundation.Infrastructure.Cms.Settings;

namespace Foundation.Features.People
{
    public class SectorsSelectionFactory : ISelectionFactory
    {
        private static readonly Lazy<ISettingsService> _settingsService = new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var settings = _settingsService.Value.GetSiteSettings<CollectionSettings>();
            return settings?.Sectors?.Select(x => new SelectItem { Value = x.Value, Text = x.Text }) ?? new List<SelectItem>();
        }
    }
}
