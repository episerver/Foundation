using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;
using Foundation.Features.Home;
using Foundation.Features.Login;
using Foundation.Features.Settings;
using Foundation.Infrastructure.Cms.Settings;
using Microsoft.AspNetCore.Mvc;
using System;

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
    }
}