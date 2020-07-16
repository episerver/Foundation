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

        private IObjectSerializer _objectSerializer;

        public PropertyListBase()
        {
            _objectSerializer = this._objectSerializerFactory.Service.GetSerializer("application/json");
        }

        protected override T ParseItem(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
