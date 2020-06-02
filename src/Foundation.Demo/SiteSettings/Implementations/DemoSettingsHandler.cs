using Foundation.Cms.SiteSettings.Pages;
using Foundation.Commerce.SiteSettings.Implementations;
using Foundation.Commerce.SiteSettings.Interfaces;
using Foundation.Demo.SiteSettings.Interfaces;
using System.Collections.Generic;

namespace Foundation.Demo.SiteSettings.Implementations
{
    public class DemoSettingsHandler : CommerceSettingsHandler
    {
        public override T CreateSettingsModel<T>(IEnumerable<SettingsBasePage> settingsPages)
        {
            if (typeof(IDemoSettingsModel).IsAssignableFrom(typeof(T)))
            {
                var model = new T() as IDemoSettingsModel;
                foreach (var s in settingsPages)
                {
                    if (s is IDemoLayoutSettings)
                    {
                        model.LayoutSettings = s as IDemoLayoutSettings;
                        continue;
                    }

                    if (s is ISiteStructureSettings)
                    {
                        model.SiteStructureSettings = s as ISiteStructureSettings;
                        continue;
                    }

                    if (s is IDemoSearchSettings)
                    {
                        model.SearchSettings = s as IDemoSearchSettings;
                        continue;
                    }
                }

                return (T)model;
            }

            return base.CreateSettingsModel<T>(settingsPages);
        }
    }
}
