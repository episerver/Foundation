using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foundation.Infrastructure.Commerce.Markets
{
    public sealed class AJAXLocalizationFilterAttribute : ActionFilterAttribute
    {
        private Injected<IUpdateCurrentLanguage> _currentLanguageUpdater = default;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                _currentLanguageUpdater.Service.UpdateLanguage(null);
            }
        }
    }
}
