using EPiServer.Social.Comments.Core;
using System;

namespace Foundation.Social.ViewModels
{
    public class ReviewViewModel
    {
        public CommentId Id { get; set; }

        public virtual bool IsVisible { get; set; }

        public EPiServer.Social.Common.Reference Parent { get; set; }

        public EPiServer.Social.Common.Reference Author { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Nickname { get; set; }

        public string Location { get; set; }

        public int Rating { get; set; }

        public DateTime AddedOn { get; set; }

        public string AddedOnStr { get; set; }
    }
}