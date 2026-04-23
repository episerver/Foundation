using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Features.Checkout.Payments
{
    public static class PaymentExtensions
    {
        private static IEnumerable<IPaymentMethod> _paymentMethods;
        private static IEnumerable<IPaymentMethod> PaymentMethods
        {
            get
            {
                if (_paymentMethods == null)
                {
                    _paymentMethods = ServiceLocator.Current.GetAllInstances<IPaymentMethod>();
                }
                return _paymentMethods;
            }
        }
        public static IPaymentMethod GetPaymentMethod(this string systemKeyword)
        {
            if (string.IsNullOrEmpty(systemKeyword))
            {
                return null;
            }

            var selectedPaymentMethod = PaymentMethods.FirstOrDefault(p => !string.IsNullOrEmpty(p.SystemKeyword) && p.SystemKeyword == systemKeyword);
            if (selectedPaymentMethod != null)
            {
                var modelType = selectedPaymentMethod.GetType();
                return ActivatorUtilities.CreateInstance(ServiceLocator.Current, modelType)as IPaymentMethod;
            }
            return null;
        }
    }
}
