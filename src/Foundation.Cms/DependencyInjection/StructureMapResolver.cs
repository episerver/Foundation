using StructureMap;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;

namespace Foundation.Cms.DependencyInjection
{
    public class StructureMapResolver : StructureMapDependencyScope, IDependencyResolver, IHttpControllerActivator
    {
        private readonly IContainer _container;

        public StructureMapResolver(IContainer container)
            : base(container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));

            _container.Inject(typeof(IHttpControllerActivator), this);
        }

        public IDependencyScope BeginScope() => new StructureMapDependencyScope(_container.GetNestedContainer());

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor,
            Type controllerType) => _container.GetNestedContainer().GetInstance(controllerType) as IHttpController;

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _container?.Dispose();

            base.Dispose(true);
        }
    }
}