using EPiServer.Events;
using System.Runtime.Serialization;

namespace Foundation.Infrastructure.Cms.Settings;

[DataContract]
[EventsServiceKnownType]
public class SettingEventData
{
    [DataMember]
    public string SiteId { get; set; }

    [DataMember]
    public string ContentId { get; set; }

    [DataMember]
    public string Language { get; set; }
}
