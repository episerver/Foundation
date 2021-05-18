using EPiServer.Data;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static Lazy<IDatabaseMode> _databaseMode = new Lazy<IDatabaseMode>(() => ServiceLocator.Current.GetInstance<IDatabaseMode>());

        public static IHtmlContent RenderReadonlyMessage(this IHtmlHelper htmlHelper)
        {
            if (_databaseMode.Value.DatabaseMode == DatabaseMode.ReadWrite)
            {
                return htmlHelper.Raw(string.Empty);
            }

            return htmlHelper.Raw(string.Format(
                "<div class=\"container-fluid px-0\"><div class=\"alert alert-info\" role=\"alert\"><p class=\"text-center\">{0}</p></div></div>",
                LocalizationService.Current.GetString(
                    "/Readonly/Message",
                    "The site is currently undergoing maintenance.Certain features are disabled until the maintenance has completed.")));
        }

        public static bool IsReadOnlyMode(this IHtmlHelper htmlHelper)
        {
            return _databaseMode.Value.DatabaseMode == DatabaseMode.ReadOnly;
        }
    }
}