using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace Foundation.Cms.DependencyInjection
{
    public class StructureMapDependencyScope : IDependencyScope
    {
        private readonly IContainer _container;

        public StructureMapDependencyScope(IContainer container) => _container = container ?? throw new ArgumentNullException(nameof(container));

        public object GetService(Type serviceType)
        {
            if (_container == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed.");
            }

            return _container.TryGetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (_container == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed.");
            }

            return _container.GetAllInstances(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _container?.Dispose();
        }
    }
}