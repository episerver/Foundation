using EPiServer;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Globalization;
using EPiServer.Tracking.Commerce;
using Foundation.Commerce.Order.Services;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;

namespace Foundation.Commerce.Personalization
{
    public class TrackingDataFactory : EPiServer.Tracking.Commerce.TrackingDataFactory
    {

        private readonly ICurrentMarket _currentMarket;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;

        public TrackingDataFactory(ILineItemCalculator lineItemCalculator,
            IContentLoader contentLoader,
            IOrderGroupCalculator orderGroupCalculator,
            LanguageResolver languageResolver,
            IOrderRepository orderRepository,
            ReferenceConverter referenceConverter,
            IRelationRepository relationRepository,
            IRecommendationContext recommendationContext,
            ICurrentMarket currentMarket,
            IRequestTrackingDataService requestTrackingDataService,
            ICartService cartService)
            : base(lineItemCalculator, contentLoader, orderGroupCalculator, languageResolver, orderRepository, referenceConverter, relationRepository, recommendationContext, currentMarket, requestTrackingDataService)
        {
            _currentMarket = currentMarket;
            _orderRepository = orderRepository;
            _cartService = cartService;
        }

        protected override IOrderGroup GetCurrentCart() => _orderRepository.LoadCart<ICart>(GetContactId(), _cartService.DefaultCartName, _currentMarket);

        protected override IOrderGroup GetCurrentWishlist() => _orderRepository.LoadCart<ICart>(GetContactId(), _cartService.DefaultWishListName, _currentMarket);
    }

}