using EPiServer.Commerce.Order;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.Payments
{
    public class PaymentModelBinderProvider : IModelBinderProvider
    {
        private static readonly IDictionary<Type, Type> ModelBinderTypeMappings = new Dictionary<Type, Type>
        {
            {typeof(IPaymentMethod), typeof(PaymentOptionViewModelBinder)},
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context) {
            if (ModelBinderTypeMappings.ContainsKey(context.Metadata.ModelType))
            {
                return context.Services.GetService(ModelBinderTypeMappings[context.Metadata.ModelType]) as IModelBinder;
            }
            return null;
        }
    }
}
