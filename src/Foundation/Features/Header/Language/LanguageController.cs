using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Markets;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Header.Language
{
    public class LanguageController : Controller
    {
        private readonly LanguageService _languageService;
        private readonly UrlResolver _urlResolver;

        public LanguageController(LanguageService languageService, UrlResolver urlResolver)
        {
            _languageService = languageService;
            _urlResolver = urlResolver;
        }

        [ChildActionOnly]
        public ActionResult Index(ContentReference contentLink, string language)
        {
            var model = new LanguageViewModel
            {
                Languages = _languageService.GetAvailableLanguages()
                    .Select(x => new SelectListItem
                    {
                        Selected = false,
                        Text = x.DisplayName,
                        Value = x.Name
                    }),
                Language = string.IsNullOrEmpty(language) ? _languageService.GetCurrentLanguage().Name : CultureInfo.GetCultureInfo(language).Name,
                ContentLink = contentLink
            };

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Set(string language, ContentReference contentLink)
        {
            _languageService.UpdateLanguage(language);

            var returnUrl = _urlResolver.GetUrl(Request, contentLink, language);
            return Json(new { returnUrl });
        }
    }
}