using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Demo.Interfaces;
using Foundation.Demo.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Foundation.Demo.ViewModels
{
    public class DemoFooterViewModelFactory : IDemoFooterViewModelFactory
    {
        private ISiteDefinitionRepository _siteDefinitionRepository;
        private UrlResolver _urlResolver;

        public DemoFooterViewModelFactory(ISiteDefinitionRepository siteDefinitionRepository, UrlResolver urlResolver)
        {
            _siteDefinitionRepository = siteDefinitionRepository;
            _urlResolver = urlResolver;
        }

        private List<SiteDefinition> GetSiteDefinitions()
        {
            var siteDefinitions = _siteDefinitionRepository.List().ToList();
            return siteDefinitions;
        }

        public IEnumerable<KeyValuePair<CultureInfo, string>> GetCurrentPageLanguages(PageData currentPage)
        {
            if (currentPage != null)
            {
                var existLanguages = currentPage.ExistingLanguages;
                foreach (var language in existLanguages)
                {
                    yield return new KeyValuePair<CultureInfo, string>(language, _urlResolver.GetUrl(currentPage.ContentLink, language.Name));
                }
            }
        }

        public DemoFooterViewModel CreateDemoFooterViewModel(DemoHomePage homePage, PageData currentPage)
        {
            var model = new DemoFooterViewModel(currentPage);
            model.HomePage = homePage;
            model.SiteDefinitions = GetSiteDefinitions();
            model.CurrentPageLanguages = GetCurrentPageLanguages(currentPage);

            return model;
        }
    }
}
