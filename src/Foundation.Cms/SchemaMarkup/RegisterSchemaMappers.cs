using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Foundation.Cms.SchemaMarkup
{
    /// <summary>
    /// Module to scan assemblies on initialisation for instances of ISchemaDataMapper and register with StructureMap
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class RegisterSchemaMappers : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.StructureMap().Configure(x =>
            {
                x.Scan(y => {
                    y.AssembliesFromApplicationBaseDirectory();
                    y.ConnectImplementationsToTypesClosing(typeof(ISchemaDataMapper<>));
                });
            });
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}