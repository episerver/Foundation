using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;

namespace Foundation.Infrastructure.Display
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (viewLocations == null)
            {
                throw new ArgumentNullException(nameof(viewLocations));
            }

            var controllerActionDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null)
            {
                throw new NullReferenceException("ControllerActionDescriptor cannot be null.");
            }


            if (!controllerActionDescriptor.Properties.ContainsKey("feature"))
            {
                return new List<string>()
                {
                    "/Views/{1}/{0}.cshtml",
                    "/Views/Shared/{0}.cshtml",
                    "/Pages/Shared/{0}.cshtml"
                };
            }

            string featureName = controllerActionDescriptor.Properties["feature"] as string;
            return ExpandViewLocations(viewLocations, featureName);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //context.Values["customviewlocation"] = nameof(FeatureViewLocationExpander);
        }

        private IEnumerable<string> ExpandViewLocations(IEnumerable<string> viewLocations, string featureName)
        {
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{3}", featureName);
            }
        }
    }
}