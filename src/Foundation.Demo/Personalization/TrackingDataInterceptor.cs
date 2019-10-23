using EPiServer;
using EPiServer.Core;
using EPiServer.Tracking.Core;
using Foundation.Demo.Models;

namespace Foundation.Demo.Personalization
{
    public class TrackingDataInterceptor : ITrackingDataInterceptor
    {
        private readonly IContentLoader _contentLoader;
        public TrackingDataInterceptor(IContentLoader contentLoader) => _contentLoader = contentLoader;
        public int SortOrder => 10;

        public void Intercept<TPayload>(TrackingData<TPayload> trackingData)
        {
            if (string.IsNullOrWhiteSpace(trackingData.Scope))
            {
                var homePage = _contentLoader.Get<PageData>(ContentReference.StartPage) as DemoHomePage;
                if (homePage != null && !string.IsNullOrWhiteSpace(homePage.TrackingScope))
                {
                    trackingData.Scope = homePage.TrackingScope;
                    return;
                }

                trackingData.Scope = "foundation";
            }
        }
    }
}