using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using System.Collections.Generic;

namespace Foundation.Social
{
    public static class CommentExtensions
    {
        private const string UserReferenceFormat = "user://{0}";
        private const string ResourceReferenceFormat = "resource://{0}";
        private static readonly ICommentService Service;

        static CommentExtensions() => Service = ServiceLocator.Current.GetInstance<ICommentService>();

        public static ResultPage<Comment> GetComments(this IContent content, Visibility visibile, int offset, int size)
        {
            var targetReference = Reference.Create(
                string.Format(ResourceReferenceFormat, content.ContentGuid.ToString()));

            var criteria = new Criteria<CommentFilter>
            {
                Filter = new CommentFilter
                {
                    Parent = targetReference,
                    Visibility = visibile
                },
                PageInfo = new PageInfo
                {
                    PageOffset = offset,
                    PageSize = size,
                    CalculateTotalCount = false
                },
                OrderBy = new List<SortInfo>
                {
                    new SortInfo(CommentSortFields.Created, false)
                }
            };

            return Service.Get(criteria);
        }

        public static Comment PublishComment(this IContent content, string authorId, string body, bool isVisible)
        {
            var authorReference = string.IsNullOrWhiteSpace(authorId)
                ? Reference.Empty
                : Reference.Create(string.Format(UserReferenceFormat, authorId));
            var targetReference = Reference.Create(string.Format(ResourceReferenceFormat, content.ContentGuid));

            var newComment = new Comment(targetReference, authorReference, body, isVisible);

            return Service.Add(newComment);
        }
    }
}