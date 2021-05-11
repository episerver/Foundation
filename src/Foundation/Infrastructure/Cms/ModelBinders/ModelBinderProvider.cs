using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Foundation.Infrastructure.Cms.ModelBinders
{
    public class ModelBinderProvider : IModelBinderProvider
    {
        private static readonly IDictionary<Type, Type> ModelBinderTypeMappings = new Dictionary<Type, Type>
        {
            {typeof(decimal), typeof(DecimalModelBinder)},
            {typeof(decimal?), typeof(DecimalModelBinder)}
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (ModelBinderTypeMappings.ContainsKey(context.Metadata.ModelType))
            {
                return context.Services.GetService(ModelBinderTypeMappings[context.Metadata.ModelType]) as IModelBinder;
            }
            return null;
        }
    }
}
