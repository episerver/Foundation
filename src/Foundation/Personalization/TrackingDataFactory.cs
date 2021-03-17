using EPiServer;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Globalization;
using EPiServer.Tracking.Commerce;
using EPiServer.Web;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;

namespace Foundation.Personalization
{
    public class TrackingDataFactory : EPiServer.Tracking.Commerce.TrackingDataFactory
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly IOrderRepository _orderRepository;

        public TrackingDataFactory(ILineItemCalculator lineItemCalculator,
            IContentLoader contentLoader,
            IOrderGroupCalculator orderGroupCalculator,
            LanguageResolver languageResolver,
            IOrderRepository orderRepository,
            ReferenceConverter referenceConverter,
            IRelationRepository relationRepository,
            IRecommendationContext recommendationContext,
            ICurrentMarket currentMarket,
            IRequestTrackingDataService requestTrackingDataService)
            : base(lineItemCalculator, contentLoader, orderGroupCalculator, languageResolver, orderRepository, referenceConverter, relationRepository, recommendationContext, currentMarket, requestTrackingDataService)
        {
            _currentMarket = currentMarket;
            _orderRepository = orderRepository;
        }

        protected override IOrderGroup GetCurrentCart() => _orderRepository.LoadCart<ICart>(GetContactId(), DefaultCartName, _currentMarket);

        protected override IOrderGroup GetCurrentWishlist() => _orderRepository.LoadCart<ICart>(GetContactId(), DefaultWishListName, _currentMarket);

        public string DefaultCartName => "Default" + SiteDefinition.Current.StartPage.ID;

        public string DefaultWishListName => "WishList" + SiteDefinition.Current.StartPage.ID;
    }
}