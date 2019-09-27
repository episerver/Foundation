using EPiServer.Data;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Cms.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString RenderReadonlyMessage(this HtmlHelper htmlHelper)
        {
            if (ServiceLocator.Current.GetInstance<IDatabaseMode>().DatabaseMode == DatabaseMode.ReadWrite)
            {
                return htmlHelper.Raw(string.Empty);
            }

            return htmlHelper.Raw(string.Format(
                "<div class=\"container-fluid\"><div class=\"alert alert-info\" role=\"alert\"><p class=\"text-center\">{0}</p></div></div>",
                LocalizationService.Current.GetString(
                    "/Readonly/Message",
                    "The site is currently undergoing maintenance.Certain features are disabled until the maintenance has completed.")));
        }
    }
}