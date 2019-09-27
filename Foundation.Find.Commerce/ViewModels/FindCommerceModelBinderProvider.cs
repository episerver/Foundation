using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Find.Commerce.ViewModels
{
    public class FindCommerceModelBinderProvider : IModelBinderProvider
    {
        private static readonly IDictionary<Type, Type> ModelBinderTypeMappings = new Dictionary<Type, Type>
        {
            {typeof(CommerceFilterOptionViewModel), typeof(CommerceFilterOptionViewModelBinder)},
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
