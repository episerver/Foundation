using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using Foundation.Commerce.Markets;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;

namespace Foundation.Features.Checkout.Payments
{
    public class CashOnDeliveryPaymentOption : PaymentOptionBase
    {
        public override string SystemKeyword => "CashOnDelivery";

        public CashOnDeliveryPaymentOption()
            : this(LocalizationService.Current, ServiceLocator.Current.GetInstance<IOrderGroupFactory>(), ServiceLocator.Current.GetInstance<ICurrentMarket>(), ServiceLocator.Current.GetInstance<LanguageService>(), ServiceLocator.Current.GetInstance<IPaymentService>())
        {
        }

        public CashOnDeliveryPaymentOption(LocalizationService localizationService,
            IOrderGroupFactory orderGroupFactory,
            ICurrentMarket currentMarket,
            LanguageService languageService,
            IPaymentService paymentService)
            : base(localizationService, orderGroupFactory, currentMarket, languageService, paymentService)
        {
        }

        public override bool ValidateData() => true;

        public override IPayment CreatePayment(decimal amount, IOrderGroup orderGroup)
        {
            var payment = orderGroup.CreatePayment(OrderGroupFactory);
            payment.PaymentType = PaymentType.Other;
            payment.PaymentMethodId = PaymentMethodId;
            payment.PaymentMethodName = SystemKeyword;
            payment.Amount = amount;
            payment.Status = PaymentStatus.Pending.ToString();
            payment.TransactionType = TransactionType.Authorization.ToString();
            return payment;
        }
    }
}