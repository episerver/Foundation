using Foundation.Features.Checkout.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Checkout.Payments
{
    public class PaymentOptionViewModelBinder : DefaultModelBinder
    {
        private readonly PaymentMethodViewModelFactory _paymentMethodViewModelFactory;

        public PaymentOptionViewModelBinder(PaymentMethodViewModelFactory paymentMethodViewModelFactory)
        {
            _paymentMethodViewModelFactory = paymentMethodViewModelFactory;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var systemKeyword = bindingContext.ValueProvider.GetValue("SystemKeyword")?.AttemptedValue;
            if (string.IsNullOrEmpty(systemKeyword))
            {
                return base.BindModel(controllerContext, bindingContext);
            }

            var paymentMethodViewModels = _paymentMethodViewModelFactory.GetPaymentMethodViewModels();
            var selectedPaymentMethod = paymentMethodViewModels.FirstOrDefault(p => p.SystemKeyword == systemKeyword);

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, selectedPaymentMethod?.PaymentOption.GetType());
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
