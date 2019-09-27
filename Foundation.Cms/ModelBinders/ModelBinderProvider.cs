using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Cms.ModelBinders
{
    public class ModelBinderProvider : IModelBinderProvider
    {
        private static readonly IDictionary<Type, Type> ModelBinderTypeMappings = new Dictionary<Type, Type>
        {
            {typeof(decimal), typeof(DecimalModelBinder)},
            {typeof(decimal?), typeof(DecimalModelBinder)}
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
