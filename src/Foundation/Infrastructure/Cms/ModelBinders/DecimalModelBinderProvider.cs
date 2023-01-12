using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Foundation.Infrastructure.Cms.ModelBinders
{
    public class DecimalModelBinderProvider : IModelBinderProvider
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
