using EPiServer.Framework.Initialization;
using Foundation.Cms.Display;
using System.Web.Mvc;

namespace Foundation.Cms.Extensions
{
    public static class InitializationEngineExtensions
    {
        public static void InitializeFoundationCms(this InitializationEngine context)
        {
            ViewEngines.Engines.Insert(0, new FeaturesViewEngine());
        }
    }
}
