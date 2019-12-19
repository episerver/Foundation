using Episerver.Marketing.Common.Helpers;
using EPiServer;
using EPiServer.ServiceLocation;
using EPiServer.Tracking.Commerce.Data;
using EPiServer.Tracking.Core;
using EPiServer.Tracking.PageView;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace Foundation.Infrastructure
{
    [ServiceConfiguration(ServiceType = typeof(ITrackingDataInterceptor), Lifecycle = ServiceInstanceScope.Singleton)]
    public class PostClickInterceptor : ITrackingDataInterceptor
    {
        public int SortOrder => int.MaxValue;

        private readonly string _postClickBaseUrl;
        private readonly string _postClickAuthToken;
        private readonly bool _isConfigured;
        private readonly ICookieHelper _cookieHelper;

        private const string TrackEventBasket = "basket";
        private const string TrackEventCategory = "category";
        private const string TrackEventOrder = "order";
        private const string TrackEventProduct = "product";
        private const string TrackEventPageView = "epiPageView";

        public PostClickInterceptor(ICookieHelper cookieHelper)
        {
            _cookieHelper = cookieHelper;

            if (ConfigurationManager.AppSettings["insightCampaign:PostClickUrl"] != null &&
                ConfigurationManager.AppSettings["insightCampaign:AuthToken"] != null)
            {
                _postClickBaseUrl = ConfigurationManager.AppSettings["insightCampaign:PostClickUrl"];
                _postClickAuthToken = ConfigurationManager.AppSettings["insightCampaign:AuthToken"];
                _isConfigured = true;
            }
        }

        public void Intercept<TPayload>(TrackingData<TPayload> trackingData)
        {
            if (_isConfigured && ShouldSendPostClick<TPayload>(trackingData))
            {

                var email = HttpUtility.UrlEncode(GetEmail(trackingData));
                switch (trackingData.EventType)
                {
                    case TrackEventPageView:
                        HostingEnvironment.QueueBackgroundWorkItem(t => TrackPageView(trackingData, email));
                        break;

                    case TrackEventBasket:
                        HostingEnvironment.QueueBackgroundWorkItem(t => TrackBasket(trackingData, email));
                        break;

                    case TrackEventCategory:
                        HostingEnvironment.QueueBackgroundWorkItem(t => TrackCategoryView(trackingData, email));
                        break;

                    case TrackEventOrder:
                        HostingEnvironment.QueueBackgroundWorkItem(t => TrackOrder(trackingData, email));
                        break;

                    case TrackEventProduct:
                        HostingEnvironment.QueueBackgroundWorkItem(t => TrackProductView(trackingData, email));
                        break;
                }
            }
        }


        private bool ShouldSendPostClick<TPayload>(TrackingData<TPayload> trackingData)
        {
            string email = GetEmail(trackingData);
            return !string.IsNullOrEmpty(email);
        }

        private string GetEmail<TPayload>(TrackingData<TPayload> trackingData)
        {
            var connectorId = EPiServer.ConnectForCampaign.Core.Helpers.Constants.ConnectorId.ToString();
            var instanceId = EPiServer.ConnectForCampaign.Core.Helpers.Constants.DefaultConnectorInstanceId.ToString();
            var email = _cookieHelper.GetTrackingCookie(connectorId, instanceId).FirstOrDefault()?.EntityId;

            if (string.IsNullOrEmpty(email))
            {
                email = trackingData?.User?.Email;
            }

            return email;
        }

        private void TrackPageView<TPayload>(TrackingData<TPayload> trackingData, string email)
        {
            var pageView = trackingData as TrackingData<EpiPageViewWrapper>;
            if (pageView != null)
            {
                PostClickRequest(email, trackingData, gvalue1: pageView.PageUri, fvalue1: 1);
            }
        }

        private void TrackProductView<TPayload>(TrackingData<TPayload> trackingData, string email)
        {
            var productView = trackingData as TrackingData<CommerceTrackingData>;
            if (productView != null)
            {
                if (productView.Payload is ProductTrackingData payload)
                {
                    PostClickRequest(email, trackingData, gvalue1: payload.Product.RefCode, gvalue2: productView.PageUri, fvalue1: 1);
                }
            }
        }

        private void TrackOrder<TPayload>(TrackingData<TPayload> trackingData, string email)
        {
            var orderEvent = trackingData as TrackingData<CommerceTrackingData>;
            if (orderEvent != null)
            {
                if (orderEvent.Payload is OrderTrackingData payload)
                {
                    var orderTotal = payload.Order.Total;
                    var orderSubtotal = payload.Order.Subtotal;
                    var orderShipping = payload.Order.Shipping;
                    var orderNo = payload.Order.OrderNo;
                    PostClickRequest(email, trackingData, gvalue1: orderNo, fvalue1: orderTotal, fvalue2: orderSubtotal, fvalue3: orderShipping);
                }
            }
        }

        private void TrackCategoryView<TPayload>(TrackingData<TPayload> trackingData, string email)
        {
            var categoryView = trackingData as TrackingData<CommerceTrackingData>;
            if (categoryView != null)
            {
                if (categoryView.Payload is CategoryTrackingData payload)
                {
                    PostClickRequest(email, trackingData, gvalue1: categoryView.PageUri, gvalue2: payload.Category, fvalue1: 1);
                }
            }
        }

        private void TrackBasket<TPayload>(TrackingData<TPayload> trackingData, string email)
        {
            var basketEvent = trackingData as TrackingData<CommerceTrackingData>;
            if (basketEvent != null)
            {
                if (basketEvent.Payload is CartTrackingData payload)
                {
                    decimal basketValue = 0;
                    int numberOfItems = 0;
                    foreach (var basketItem in payload.Basket.Items)
                    {
                        basketValue += basketItem.Price * basketItem.Quantity;
                        numberOfItems += basketItem.Quantity;
                    }
                    PostClickRequest(email, trackingData, fvalue1: basketValue, fvalue2: numberOfItems, gvalue1: payload.Basket.Currency);
                }
            }
        }

        private void PostClickRequest<TPayload>(
            string email,
            TrackingData<TPayload> trackingData,
            string gvalue1 = null,
            string gvalue2 = null,
            decimal? fvalue1 = null,
            decimal? fvalue2 = null,
            decimal? fvalue3 = null
        )
        {
            var urlBuilder = new UrlBuilder(_postClickBaseUrl);
            urlBuilder.QueryCollection.Add("type", "userEvent");
            urlBuilder.QueryCollection.Add("authToken", _postClickAuthToken);
            urlBuilder.QueryCollection.Add("recipientId", email);
            urlBuilder.QueryCollection.Add("service", trackingData.EventType);
            urlBuilder.QueryCollection.Add("bi", "0");

            if (!string.IsNullOrEmpty(gvalue1)) urlBuilder.QueryCollection.Add("gvalue1", gvalue1);
            if (!string.IsNullOrEmpty(gvalue2)) urlBuilder.QueryCollection.Add("gvalue2", gvalue2);
            if (fvalue1.HasValue) urlBuilder.QueryCollection.Add("fvalue1", fvalue1.Value.ToString());
            if (fvalue2.HasValue) urlBuilder.QueryCollection.Add("fvalue2", fvalue2.Value.ToString());
            if (fvalue3.HasValue) urlBuilder.QueryCollection.Add("fvalue3", fvalue3.Value.ToString());

            urlBuilder.QueryCollection.Add("bmEncoding", "utf-8");

            var request = (HttpWebRequest)WebRequest.Create(urlBuilder.ToString());
            request.Method = WebRequestMethods.Http.Get;
            request.GetResponse();
        }
    }
}