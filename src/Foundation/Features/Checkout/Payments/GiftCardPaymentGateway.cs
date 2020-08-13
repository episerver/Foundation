using EPiServer.Commerce.Order;
using Foundation.Commerce.GiftCard;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace Foundation.Features.Checkout.Payments
{
    public class GiftCardPaymentGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        public PaymentProcessingResult ProcessPayment(IOrderGroup orderGroup, IPayment payment)
        {
            if (orderGroup == null)
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("Failed to process your payment.");
            }
            else
            {
                GiftCardManager.PurchaseByGiftCard(payment, orderGroup.Currency);
                return PaymentProcessingResult.CreateSuccessfulResult("Gift card processed");
            }
        }

        public override bool ProcessPayment(Payment payment, ref string message)
        {
            var result = ProcessPayment(null, payment);
            message = result.Message;
            return result.IsSuccessful;
        }
    }
}