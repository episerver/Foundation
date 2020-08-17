using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Foundation.Infrastructure.Display
{
    public class FeaturesViewEngine : RazorViewEngine
    {
        private static readonly string[] AdditionalPartialViewFormats =
        {
            "~/Features/Blocks/{0}.cshtml",
            "~/Features/Blocks/{1}/{0}.cshtml",
            "~/Features/Shared/{0}.cshtml",
            "~/Features/Shared/Views/{0}.cshtml",
            "~/Features/Shared/Views/{1}/{0}.cshtml",
            "~/Features/Shared/Views/Header/{0}.cshtml",
        };

        public FeaturesViewEngine()
        {
            ViewLocationCache = new DefaultViewLocationCache();

            var featureFolders = new[]
            {
                "~/Features/%1/{1}/{0}.cshtml",
                "~/Features/%1/{0}.cshtml"
            };

            featureFolders = featureFolders.Union(AdditionalPartialViewFormats).ToArray();

            ViewLocationFormats = ViewLocationFormats
                .Union(featureFolders)
                .ToArray();

            PartialViewLocationFormats = PartialViewLocationFormats
                .Union(featureFolders)
                .ToArray();

            MasterLocationFormats = MasterLocationFormats
                .Union(featureFolders)
                .ToArray();
        }

        private string GetFeatureName(TypeInfo controllerType)
        {
            var tokens = controllerType.FullName?.Split('.');
            if (!tokens?.Any(t => t == "Features") ?? true)
            {
                return "";
            }

            return tokens
                .SkipWhile(t => !t.Equals("features",
                    StringComparison.CurrentCultureIgnoreCase))
                .Skip(1)
                .Take(1)
                .FirstOrDefault();
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            if (controllerContext.Controller != null)
            {
                return base.CreatePartialView(controllerContext,
                partialPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo())));
            }

            return base.CreatePartialView(controllerContext, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(controllerContext,
                viewPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo())),
                masterPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo())));
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            if (controllerContext.Controller == null)
            {
                return base.FileExists(controllerContext, virtualPath);
            }

            return base.FileExists(controllerContext,
                virtualPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo())));
        }
    }
}