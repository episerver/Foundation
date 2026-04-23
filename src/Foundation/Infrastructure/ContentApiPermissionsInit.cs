using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace Foundation.Infrastructure
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ContentApiPermissionsInit : IInitializableModule
    {
        private IContentLoader _contentLoader = null;
        public void Initialize(InitializationEngine context)
        {
            ServiceProviderHelper serviceLocationHelper = context.Locate;
            _contentLoader = serviceLocationHelper.ContentLoader();

            var contentLoader = context.Locate.Advanced.GetInstance<IContentLoader>();
            var contentSecurityRepository = context.Locate.Advanced.GetInstance<IContentSecurityRepository>();

            PageData rootPage = contentLoader.Get<PageData>(ContentReference.RootPage);
            IContentSecurityDescriptor securityDescriptor = (IContentSecurityDescriptor)contentSecurityRepository.Get(rootPage.ContentLink).CreateWritableClone();

            securityDescriptor.AddEntry(new AccessControlEntry("postman-client", 
                AccessLevel.Read | AccessLevel.Create | AccessLevel.Edit | AccessLevel.Delete | AccessLevel.Publish, 
                SecurityEntityType.Application));
            contentSecurityRepository.Save(rootPage.ContentLink, securityDescriptor, SecuritySaveType.Replace);
        }
        public void Uninitialize(InitializationEngine initializationEngine)
        {
            //
        }
    }
}