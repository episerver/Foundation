using EPiServer;
using EPiServer.Cms.Shell.Extensions;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Cms.SiteSettings.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.EditorDescriptors
{
    public class ColorOptionsSelectionFactory : ISelectionFactory
    {
        private readonly Injected<ISiteSettingsProvider> _siteSettingsProvider;
        private readonly Injected<IContentLoader> _contentLoader;

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var currentContent = metadata.FindOwnerContent();
            var currentPage = currentContent;
            if (currentContent is BlockData)
            {
                ContentAssetFolder assetFolder;
                PageData page;
                if (!_contentLoader.Service.TryGet(currentPage.ParentLink, out assetFolder))
                {
                    currentPage = _contentLoader.Service.Get<IContent>(ContentReference.StartPage);
                }
                else if (!_contentLoader.Service.TryGet(assetFolder.ContentOwnerID, out page))
                {
                    currentPage = _contentLoader.Service.Get<IContent>(ContentReference.StartPage);
                }
                else
                {
                    currentPage = page;
                }
            }

            return GetColorOptions(currentPage);
        }

        private IEnumerable<ISelectItem> GetColorOptions(IContent currentPage)
        {
            var homePage = _contentLoader.Service.Get<CmsHomePage>(currentPage.GetRelativeStartPage());
            var colorOptions = homePage.ColorOptions;

            if (colorOptions != null)
            {
                return colorOptions.Select(x => new SelectItem { Text = x.ColorName, Value = x.ColorCode });
            }
            else
            {
                return new List<SelectItem>();
            }
        }
    }
}