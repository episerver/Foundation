using EPiServer.Data;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using System;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Cms.Extensions
{
    public static class HtmlHelperExtensions
    {
        private static Lazy<IDatabaseMode> _databaseMode = new Lazy<IDatabaseMode>(() => ServiceLocator.Current.GetInstance<IDatabaseMode>());

        public static IHtmlString RenderReadonlyMessage(this HtmlHelper htmlHelper)
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

        public static bool IsReadOnlyMode(this HtmlHelper htmlHelper)
        {
            return _databaseMode.Value.DatabaseMode == DatabaseMode.ReadOnly;
        }
    }
}