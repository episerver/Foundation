using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Foundation.Features.Checkout.Payments
{
    public class PaymentOptionViewModelBinder : IModelBinder
    {
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;
        private readonly IModelMetadataProvider _defaultProvider;

        public PaymentOptionViewModelBinder(IEnumerable<IPaymentMethod> paymentMethods, IModelMetadataProvider defaultProvider)
        {
            _paymentMethods = paymentMethods;
            _defaultProvider = defaultProvider;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string valueFromBody;
            using (var streamReader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                valueFromBody = await streamReader.ReadToEndAsync();
            }

            var systemKeyword = "";
            if (!string.IsNullOrEmpty(valueFromBody))
            {
                var jObject = JObject.Parse(valueFromBody);
                systemKeyword = jObject["SystemKeyword"].ToString();
            }
            var selectedPaymentMethod = _paymentMethods.FirstOrDefault(p => !string.IsNullOrEmpty(p.SystemKeyword) && p.SystemKeyword == systemKeyword);
            if (selectedPaymentMethod != null)
            {
                var modelType = selectedPaymentMethod.GetType();
                var model = ActivatorUtilities.CreateInstance(ServiceLocator.Current, modelType);
                bindingContext.ModelMetadata = _defaultProvider.GetMetadataForType(modelType);
                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
    }
}
