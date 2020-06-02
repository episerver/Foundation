using Foundation.Cms.SiteSettings.Implementations;
using Foundation.Cms.SiteSettings.Pages;
using Foundation.Commerce.SiteSettings.Interfaces;
using System.Collections.Generic;

namespace Foundation.Commerce.SiteSettings.Implementations
{
    public class CommerceSettingsHandler : CmsSettingsHandler
    {
        public override T CreateSettingsModel<T>(IEnumerable<SettingsBasePage> settingsPages)
        {
            if (typeof(ICommerceSettingsModel).IsAssignableFrom(typeof(T))) {
                var model = new T() as ICommerceSettingsModel;
                foreach (var s in settingsPages)
                {
                    if (s is ICommerceLayoutSettings)
                    {
                        model.LayoutSettings = s as ICommerceLayoutSettings;
                        continue;
                    }

                    if (s is ISiteStructureSettings)
                    {
                        model.SiteStructureSettings = s as ISiteStructureSettings;
                        continue;
                    }
                }

                return (T)model;
            }

            return base.CreateSettingsModel<T>(settingsPages);
        }
    }
}
