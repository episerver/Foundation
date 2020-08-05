using EPiServer.Core;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Features.Blog.BlogItemPage;
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
            var posting = new BlogPosting
            {
                Headline = content.Name,
                Description = content.TeaserText ?? content.PageDescription ?? string.Empty,
                Author = new Person
                {
                    Name = content.Author ?? string.Empty
                },
                DatePublished = new DateTimeOffset(content.StartPublish ?? content.Changed),
                DateModified = new DateTimeOffset(content.Changed)
            };

            if (content != null && !ContentReference.IsNullOrEmpty(content.PageImage))
            {
                posting.Image = (content.PageImage.Get<MediaData>() as MediaData)?.GetUri(true);
            }

            return posting;
        }
    }
}
