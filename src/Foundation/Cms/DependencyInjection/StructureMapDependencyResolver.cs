using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Cms.DependencyInjection
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container) => _container = container;

        public object GetService(Type serviceType)
        {
            if (serviceType.IsInterface || serviceType.IsAbstract)
            {
                return GetInterfaceService(serviceType);
            }

            return GetConcreteService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) => _container.GetAllInstances(serviceType).Cast<object>();

        private object GetConcreteService(Type serviceType) => _container.GetInstance(serviceType);

        private object GetInterfaceService(Type serviceType) => _container.TryGetInstance(serviceType);
    }
}