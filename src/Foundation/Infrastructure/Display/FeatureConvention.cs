using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Linq;
using System.Reflection;

namespace Foundation.Infrastructure.Display
{
    public class FeatureConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Properties.Add("feature",
              GetFeatureName(controller.ControllerType));
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
    }
}
