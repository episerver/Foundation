using EPiServer.Social.Comments.Core;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Social.Services
{
    public interface ICommentManagerService
    {
        IEnumerable<ReviewViewModel> Get(int page, int limit, out long total);
        void Delete(string id);
        Comment Approve(string id);
    }
}