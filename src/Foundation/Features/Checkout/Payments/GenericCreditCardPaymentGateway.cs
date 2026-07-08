using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace Foundation.Features.Checkout.Payments
{
    public class GenericCreditCardPaymentGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        public PaymentProcessingResult ProcessPayment(IOrderGroup orderGroup, IPayment payment)
        {
            // Commerce 15 removed ICreditCardPayment: GenericCreditCardPaymentOption now creates
            // a generic IPayment, so casting to the (stubbed) ICreditCardPayment threw
            // InvalidCastException at runtime and made every credit-card purchase fail.
            // The cast was dead code — the validation that used it is commented out below.
            return PaymentProcessingResult.CreateSuccessfulResult("");
            //if (creditCardPayment.CreditCardNumber.EndsWith("4"))
            //{
            //    return PaymentProcessingResult.CreateUnsuccessfulResult("Invalid credit card number.");
            //}
            //else { 
            //    return PaymentProcessingResult.CreateSuccessfulResult("");
            //}
        }

        /// <inheritdoc />
        public override bool ProcessPayment(Payment payment, ref string message)
        {
            var result = ProcessPayment(null, payment);
            message = result.Message;
            return result.IsSuccessful;
        }
    }
}