using EPiServer.Core;
using Schema.NET;

namespace Foundation.Infrastructure.Cms
{
    /// <summary>
    /// Interface for mapping CMS content to Schema.org types
    /// </summary>
    public interface ISchemaDataMapper<T> where T : IContent
    {
        Thing Map(T content);
    }
}
