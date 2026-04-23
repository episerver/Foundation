using Microsoft.AspNetCore.Mvc.Controllers;

namespace Foundation.Infrastructure.Display
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        private const string ChildFeature = "childFeature";
        private const string Feature = "feature";
        private readonly List<string> _viewLocationFormats = new List<string>()
        {
            "/Features/Shared/{0}.cshtml",
            "/Features/Blocks/{0}.cshtml",
            "/Features/Blocks/{1}/{0}.cshtml",
            "/Features/Shared/Views/{0}.cshtml",
            "/Features/Shared/Views/{1}/{0}.cshtml",
            "/Features/Shared/Views/Header/{0}.cshtml",
            "/Cms/Views/{1}/{0}.cshtml",
            "/Features/{3}/{1}/{0}.cshtml",
            "/Features/{3}/{0}.cshtml",
            "/Features/{3}/{4}/{1}/{0}.cshtml",
            "/Features/{3}/{4}/{0}.cshtml",
            "/Features/Shared/Views/ElementBlocks/{0}.cshtml"
        };

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
            if (controllerActionDescriptor != null && controllerActionDescriptor.Properties.ContainsKey(Feature))
            {
                string featureName = controllerActionDescriptor.Properties[Feature] as string;
                string childFeatureName = null;
                if (controllerActionDescriptor.Properties.ContainsKey(ChildFeature))
                {
                    childFeatureName = controllerActionDescriptor.Properties[ChildFeature] as string;
                }
                foreach (var item in ExpandViewLocations(_viewLocationFormats.Union(viewLocations), featureName, childFeatureName))
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var location in viewLocations)
                {
                    yield return location;
                }
            }
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controllerActionDescriptor = context.ActionContext?.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null || !controllerActionDescriptor.Properties.ContainsKey(Feature))
            {
                return;
            }
            context.Values[Feature] = controllerActionDescriptor?.Properties[Feature].ToString();

            if (controllerActionDescriptor.Properties.ContainsKey(ChildFeature))
            {
                context.Values[ChildFeature] = controllerActionDescriptor?.Properties[ChildFeature].ToString();
            }
        }

        private IEnumerable<string> ExpandViewLocations(IEnumerable<string> viewLocations, 
            string featureName,
            string childFeatureName)
        {
            foreach (var location in viewLocations)
            {
                var updatedLocation = location.Replace("{3}", featureName);
                if (location.Contains("{4}") && string.IsNullOrEmpty(childFeatureName))
                {
                    continue;
                }
                else
                {
                    updatedLocation = updatedLocation.Replace("{4}", childFeatureName);
                }
                yield return updatedLocation;
            }
        }
    }
}