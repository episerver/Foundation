using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Find.Cms.ViewModels
{
    public class FindCmsModelBinderProvider : IModelBinderProvider
    {
        private static readonly IDictionary<Type, Type> ModelBinderTypeMappings = new Dictionary<Type, Type>
        {
            {typeof(CmsFilterOptionViewModel), typeof(CmsFilterOptionViewModelBinder)},
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
