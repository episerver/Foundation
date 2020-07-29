using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using Foundation.Commerce.Markets;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;

namespace Foundation.Features.Checkout.Payments
{
    public class BudgetPaymentOption : PaymentOptionBase
    {
        private readonly IOrderGroupFactory _orderGroupFactory;

        public BudgetPaymentOption()
            : this(LocalizationService.Current, ServiceLocator.Current.GetInstance<IOrderGroupFactory>(), ServiceLocator.Current.GetInstance<ICurrentMarket>(), ServiceLocator.Current.GetInstance<LanguageService>(), ServiceLocator.Current.GetInstance<IPaymentService>())
        {
        }

        public BudgetPaymentOption(LocalizationService localizationService,
            IOrderGroupFactory orderGroupFactory,
            ICurrentMarket currentMarket,
            LanguageService languageService,
            IPaymentService paymentService)
            : base(localizationService, orderGroupFactory, currentMarket, languageService, paymentService)
        {
            _orderGroupFactory = orderGroupFactory;
        }

        public override string SystemKeyword { get; } = "BudgetPayment";

        public override IPayment CreatePayment(decimal amount, IOrderGroup orderGroup)
        {
            var payment = _orderGroupFactory.CreatePayment(orderGroup);
            payment.PaymentMethodId = PaymentMethodId;
            payment.PaymentMethodName = "BudgetPayment";
            payment.Amount = amount;
            payment.Status = PaymentStatus.Pending.ToString();
            payment.TransactionType = TransactionType.Authorization.ToString();
            return payment;
        }

        public override bool ValidateData() => true;
    }
}