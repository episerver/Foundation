using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Features.Blocks.ODP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Foundation.Features.Blocks.JourneyContainerBlock
{
    [ModuleDependency(typeof(Infrastructure.Cms.Initialize))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class InitializeJourneyContainerBlock : IConfigurableModule
    {
        private IServiceProvider _locator;
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
        }

        public void Initialize(InitializationEngine context)
        {
            _locator = context.Locate.Advanced;
            context.InitComplete += ContextOnInitComplete;
        }

        private void ContextOnInitComplete(object sender, EventArgs eventArgs)
        {
            _locator.GetInstance<IContentEvents>().PublishedContent += OnPublishedContent;
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.Advanced.GetInstance<IContentEvents>().PublishedContent -= OnPublishedContent;
        }

        private void OnPublishedContent(object sender, ContentEventArgs contentEventArgs)
        {
            var content = contentEventArgs.Content as JourneyContainerBlock;
            if (content != null)
            {
                DateTime eventTime = content.JourneyStartTime;
                IContentRepository contentRepository = ServiceLocator.Current.GetService<IContentRepository>();
                if (content.MainContentArea != null && content.MainContentArea.Items != null && content.MainContentArea.Items.Any())
                {
                    foreach (var item in content.MainContentArea.Items)
                    {
                        // CMS 13: ContentAreaItem.GetContent() removed. Use IContentRepository.Get<IContent>() instead.
                        var block = contentRepository.Get<IContent>(item.ContentLink) as BaseODPEventBlock;
                        if (block != null)
                        {
                            var writeableBlock = block.CreateWritableClone() as BaseODPEventBlock;
                            writeableBlock.EventTime = eventTime;
                            contentRepository.Publish((IContent)writeableBlock);
                            eventTime = eventTime.AddMinutes(2);
                        }
                    }
                }
            }
        }
    }
}
