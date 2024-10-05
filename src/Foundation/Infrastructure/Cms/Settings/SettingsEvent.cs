using EPiServer.Events;
using System.Runtime.Serialization;

namespace Foundation.Infrastructure.Cms.Settings
{
    [DataContract]
    [EventsServiceKnownType]
    public class SettingsEvent
    {
        [DataMember]
        public Guid SiteId { get; set; }

        [DataMember]
        public string ContentReference { get; set; }

        [DataMember]
        public bool IsPublished { get; set; }

        [DataMember]
        public bool IsDelete { get; set; }
    }
}
