using EPiServer.Commerce.Order;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Checkout.Payments
{
    public class PaymentModelBinderProvider : IModelBinderProvider
    {
        private static readonly IDictionary<Type, Type> ModelBinderTypeMappings = new Dictionary<Type, Type>
        {
            {typeof(IPaymentMethod), typeof(PaymentOptionViewModelBinder)},
        };

        public IModelBinder GetBinder(Type modelType)
        {
            if (ModelBinderTypeMappings.ContainsKey(modelType))
            {
                return DependencyResolver.Current.GetService(ModelBinderTypeMappings[modelType]) as IModelBinder;
            }

            return null;
        }
    }
}
