using EPiServer.Core;
using EPiServer.Framework.Serialization;
using EPiServer.Framework.Serialization.Internal;
using EPiServer.ServiceLocation;
using Newtonsoft.Json;

namespace Foundation.Find.Facets.Config
{
    public class PropertyListBase<T> : PropertyList<T>
    {
        private Injected<ObjectSerializerFactory> _objectSerializerFactory;

        private readonly IObjectSerializer _objectSerializer;

        public PropertyListBase() => _objectSerializer = _objectSerializerFactory.Service.GetSerializer("application/json");

        protected override T ParseItem(string value) => JsonConvert.DeserializeObject<T>(value);
    }
}
