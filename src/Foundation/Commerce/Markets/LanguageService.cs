using EPiServer.Core;
using EPiServer.Globalization;
using Foundation.Cms;
using Mediachase.Commerce;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Foundation.Commerce.Markets
{
    public class LanguageService : IUpdateCurrentLanguage
    {
        private const string LanguageCookie = "Language";
        private readonly CookieService _cookieService;
        private readonly ICurrentMarket _currentMarket;
        private readonly IUpdateCurrentLanguage _defaultUpdateCurrentLanguage;

        public LanguageService(
            ICurrentMarket currentMarket,
            CookieService cookieService,
            IUpdateCurrentLanguage defaultUpdateCurrentLanguage)
        {
            _currentMarket = currentMarket;
            _cookieService = cookieService;
            _defaultUpdateCurrentLanguage = defaultUpdateCurrentLanguage;
        }

        private IMarket CurrentMarket => _currentMarket.GetCurrentMarket();

        public void UpdateLanguage(string languageId)
        {
            var chosenLanguage = languageId;
            var cookieLanguage = _cookieService.Get(LanguageCookie);

            if (string.IsNullOrEmpty(chosenLanguage))
            {
                if (cookieLanguage != null)
                {
                    chosenLanguage = cookieLanguage;
                }
                else
                {
                    var currentMarket = _currentMarket.GetCurrentMarket();
                    if (currentMarket != null && currentMarket.DefaultLanguage != null)
                    {
                        chosenLanguage = currentMarket.DefaultLanguage.Name;
                    }
                }
            }

            _defaultUpdateCurrentLanguage.UpdateLanguage(chosenLanguage);

            if (cookieLanguage == null || cookieLanguage != chosenLanguage)
            {
                _cookieService.Set(LanguageCookie, chosenLanguage);
            }
        }

        public void UpdateReplacementLanguage(IContent currentContent, string requestedLanguage) => _defaultUpdateCurrentLanguage.UpdateReplacementLanguage(currentContent, requestedLanguage);

        public virtual IEnumerable<CultureInfo> GetAvailableLanguages() => CurrentMarket.Languages;

        public virtual CultureInfo GetCurrentLanguage()
        {
            return TryGetLanguage(_cookieService.Get(LanguageCookie), out var cultureInfo)
                ? cultureInfo
                : CurrentMarket.DefaultLanguage;
        }

        private bool TryGetLanguage(string language, out CultureInfo cultureInfo)
        {
            cultureInfo = null;

            if (language == null)
            {
                return false;
            }

            try
            {
                var culture = CultureInfo.GetCultureInfo(language);
                cultureInfo = GetAvailableLanguages().FirstOrDefault(c => c.Name == culture.Name);
                return cultureInfo != null;
            }
            catch (CultureNotFoundException)
            {
                return false;
            }
        }
    }
}