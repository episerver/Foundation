using Foundation.Features.Home;
using Foundation.Features.Login;
using Foundation.Infrastructure.Cms.Settings;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Foundation.Infrastructure.Helpers
{
    public static class Extensions
    {
        private static readonly Lazy<ISettingsService> _settingsService =
            new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());

        private static readonly Lazy<IContentLoader> ContentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        public static UserViewModel GetUserViewModel(this IUrlHelper urlHelper, string returnUrl, string title = "Login")
        {
            var referencePages = _settingsService.Value.GetSiteSettings<ReferencePageSettings>();
            var layoutpages = _settingsService.Value.GetSiteSettings<LayoutSettings>();

            var model = new UserViewModel();
            ContentLoader.Value.TryGet(ContentReference.StartPage, out HomePage homePage);
            model.Logo = urlHelper.ContentUrl(layoutpages?.SiteLogo);
            model.ResetPasswordUrl = urlHelper.ContentUrl(referencePages?.ResetPasswordPage);
            model.Title = title;
            model.LoginViewModel.ReturnUrl = returnUrl;
            return model;
        }

        public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}