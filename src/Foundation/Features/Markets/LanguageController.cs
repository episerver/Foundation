using Newtonsoft.Json;

namespace Foundation.Features.Markets
{
    [ApiController]
    [Route("[controller]")]
    public class LanguageController : ControllerBase
    {
        private readonly LanguageService _languageService;
        private readonly UrlResolver _urlResolver;
        private readonly IContentRouteHelper _contentRouteHelper;

        public LanguageController(LanguageService languageService, UrlResolver urlResolver, IContentRouteHelper contentRouteHelper)

        {
            _languageService = languageService;
            _urlResolver = urlResolver;
            _contentRouteHelper = contentRouteHelper;
        }

        [HttpPost]
        [Route("Set")]
        public ActionResult Set([FromForm] string language, ContentReference contentLink)
        {
            _languageService.SetRoutedContent(_contentRouteHelper.Content, language);

            var returnUrl = _urlResolver.GetUrl(Request, contentLink, language);
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(new { returnUrl }),
                ContentType = "application/json",
            };
        }
    }
}