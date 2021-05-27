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
            "~/Features/Shared/Views/{0}.cshtml",
            "~/Features/Shared/Views/{1}/{0}.cshtml",
            "~/Features/Shared/Views/Header/{0}.cshtml",
            "~/Cms/Views/{1}/{0}.cshtml",
            "~/Commerce/Views/{1}/{0}.cshtml",
            "~/Social/Views/{1}/{0}.cshtml"
        };

        public FeaturesViewEngine()
        {
            ViewLocationCache = new DefaultViewLocationCache();

            var featureFolders = new[]
            {
                "~/Features/%1/{1}/{0}.cshtml",
                "~/Features/%1/%2/{1}/{0}.cshtml",
                "~/Features/%1/{0}.cshtml",
                "~/Features/%1/%2/{0}.cshtml",
                "~/Cms/%1/{1}/{0}.cshtml",
                "~/Commerce/%1/{1}/{0}.cshtml",
                "~/Social/%1/{1}/{0}.cshtml"
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

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            if (controllerContext.Controller != null)
            {
                partialPath = partialPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
                if (partialPath.IndexOf("%2") > 0)
                {
                    partialPath = partialPath.Replace("%2", GetChildFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
                }

                return base.CreatePartialView(controllerContext, partialPath);
            }

            return base.CreatePartialView(controllerContext, partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            viewPath = viewPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
            if (viewPath.IndexOf("%2") > 0)
            {
                viewPath = viewPath.Replace("%2", GetChildFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
            }

            masterPath = masterPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
            if (masterPath.IndexOf("%2") > 0)
            {
                masterPath = masterPath.Replace("%2", GetChildFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
            }

            return base.CreateView(controllerContext, viewPath, masterPath);
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            if (controllerContext.Controller == null)
            {
                return base.FileExists(controllerContext, virtualPath);
            }

            virtualPath = virtualPath.Replace("%1", GetFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
            if (virtualPath.IndexOf("%2") > 0)
            {
                virtualPath = virtualPath.Replace("%2", GetChildFeatureName(controllerContext.Controller.GetType().GetTypeInfo()));
            }

            return base.FileExists(controllerContext, virtualPath);
        }
    }
}