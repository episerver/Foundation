using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace Foundation.Infrastructure.Display
{
    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("feature", GetFeatureName(controller.ControllerType));
            controller.Properties.Add("childFeature", GetChildFeatureName(controller.ControllerType));
        }
        private string GetFeatureName(TypeInfo controllerType)
        {
            string[] tokens = controllerType.FullName.Split('.');
            if (!tokens.Any(t => t == "Features")) return "";
            return tokens
              .SkipWhile(t => !t.Equals("features",
                StringComparison.CurrentCultureIgnoreCase))
              .Skip(1)
              .Take(1)
              .FirstOrDefault();
        }

        private string GetChildFeatureName(TypeInfo controllerType)
        {
            var tokens = controllerType.FullName?.Split('.');
            if (!tokens?.Any(t => t == "Features") ?? true)
            {
                return "";
            }

            return tokens
                .SkipWhile(t => !t.Equals("features",
                    StringComparison.CurrentCultureIgnoreCase))
                .Skip(2)
                .Take(1)
                .FirstOrDefault();
        }
    }
}
