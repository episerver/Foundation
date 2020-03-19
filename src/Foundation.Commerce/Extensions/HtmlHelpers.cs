using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Foundation.Commerce.Models.Pages;
using System;
using System.Web.Mvc;

namespace Foundation.Commerce.Extensions
{
    public static class HtmlHelpers
    {
        private static readonly Lazy<IContentLoader> ContentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        public static ContentReference GetSearchPage(this HtmlHelper helper) => ContentLoader.Value.Get<CommerceHomePage>(ContentReference.StartPage).SearchPage;
    }
}