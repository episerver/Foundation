using EPiServer.Core;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Extensions;
using Schema.NET;
using System;

namespace Foundation.Infrastructure.SchemaMarkup
{
    /// <summary>
    /// Map a BlogItemPage to a schema.org blog posting
    /// </summary>
    public class BlogItemPageSchemaMapper : ISchemaDataMapper<BlogItemPage>
    {
        public Thing Map(BlogItemPage content)
        {
            return new BlogPosting
            {
                Headline = content.Name,
                Description = content.TeaserText ?? content.PageDescription ?? string.Empty,
                Image = (content?.PageImage?.Get<MediaData>() as MediaData)?.GetUri(true) ?? new Uri(string.Empty),
                Author = new Person
                {
                    Name = content.Author ?? string.Empty
                },
                DatePublished = new DateTimeOffset(content.StartPublish ?? content.Changed),
                DateModified = new DateTimeOffset(content.Changed)
            };
        }
    }
}