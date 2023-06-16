using Mediachase.Commerce.Anonymous;
using Mediachase.Commerce.Markets;

namespace Infrastructure.Commerce.Extensions
{
    public class AnonymousCartMergingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICurrentMarket _currentMarket;

        public AnonymousCartMergingMiddleware(RequestDelegate next, ICurrentMarket currentMarket)
        {
            _next = next;
            _currentMarket = currentMarket;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var anonymousId = context.Features.Get<IAnonymousIdFeature>().AnonymousId;

                if (!string.IsNullOrWhiteSpace(anonymousId))
                {
                    var orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();
                    var marketService = ServiceLocator.Current.GetInstance<IMarketService>();

                    var currentMarket = _currentMarket.GetCurrentMarket();
                    var cart = orderRepository.LoadCart<ICart>(new Guid(anonymousId), DefaultCartName, currentMarket.MarketId);

                    if (cart != null && cart.GetAllLineItems().ToList().Count > 0)
                    {
                        cart.MarketId = currentMarket.MarketId;
                        orderRepository.Save(cart);

                        var profileMigrator = ServiceLocator.Current.GetInstance<IProfileMigrator>();
                        profileMigrator.MigrateCarts(new Guid(anonymousId));
                    }
                }
            }
            await _next(context);
        }
        public string DefaultCartName => "Default" + SiteDefinition.Current.StartPage.ID;
    }
}