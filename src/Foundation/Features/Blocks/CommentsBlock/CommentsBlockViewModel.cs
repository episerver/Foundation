using EPiServer.Core;
using Foundation.Social;
using Foundation.Social.Models.Comments;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.CommentsBlock
{
    public class CommentsBlockViewModel
    {
        public CommentsBlockViewModel(CommentsBlock block, PageReference pageReference)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            CommentBoxRows = block.CommentBoxRows;
            CommentMaxLength = block.CommentMaxLength;
            CommentsDisplayMax = block.CommentsDisplayMax;
            Comments = new List<PageComment>();
            SendActivity = block.SendActivity;
            CurrentPageLink = pageReference;
            CurrentBlock = block;
        }

        public PageReference CurrentPageLink { get; set; }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public int CommentBoxRows { get; set; }

        public int CommentMaxLength { get; set; }

        public int CommentsDisplayMax { get; set; }

        public IEnumerable<PageComment> Comments { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public bool SendActivity { get; }

        public CommentsBlock CurrentBlock { get; set; }
    }
}