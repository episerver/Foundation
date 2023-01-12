using Advanced.CMS.ExternalReviews;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.ExternalReviews
{
    [InitializableModule]
    [ModuleDependency(typeof(FrameworkInitialization))]
    public class ExternalReviewInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.Configure<ExternalReviewOptions>(options =>
            {
                options.EditableLinksEnabled = true;
                options.PinCodeSecurity.Enabled = true;
                options.PinCodeSecurity.CodeLength = 4;
            });
        }

        public void Initialize(InitializationEngine context) { }

        public void Uninitialize(InitializationEngine context) { }
    }
}
